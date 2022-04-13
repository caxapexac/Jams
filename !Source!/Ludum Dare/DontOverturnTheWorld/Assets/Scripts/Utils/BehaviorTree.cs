using System;


namespace Common.Behaviors
{
    public enum BTResult : byte
    {
        Success = 0b00000001,
        Fail = 0b00000010,
        Pending = 0b00000100,
        Error = 0b00000111,
    }


    internal enum BTStateFlags : byte
    {
        ResultMask = 0b00000111,
        DataMask = 0b11111000,
        IsRight = 0b10000000,
    }


    public delegate BTResult BTNodeFunc<T>(ref T payload);


    [Flags]
    public enum BTNodeFlags : byte
    {
        Error = 0b00000000,
        Sequence = 0b00000010, // Неудача при первой неудачи
        Selector = 0b00000001, // Удача при первой удачи
        Parallel = 0b10000000, // Первая удача или неудача
        TypeMask = 0b00000011,
        LeftFunc = 0b00000100,
        RighFunc = 0b00001000,
        NotRoot = 0b00010000,
        IsLeft = 0b00100000,
        IsRigh = 0b01000000,
    }


    [Serializable]
    public struct BTState
    {
        public int NodeIndex;
        internal BTStateFlags Flags;

        public bool IsRight
        {
            get => (Flags & BTStateFlags.IsRight) != 0;
            set => Flags = (Flags ^ (Flags & BTStateFlags.IsRight)) | (value ? BTStateFlags.IsRight : 0);
        }

        public BTResult Result
        {
            get => (BTResult)(Flags & BTStateFlags.ResultMask);
            set => Flags = (Flags ^ (Flags & BTStateFlags.ResultMask)) | (BTStateFlags)value;
        }

        public BTState(int nodeIndex)
        {
            NodeIndex = nodeIndex;
            Flags = 0;
        }
    }


    internal struct BTNode
    {
        public BTNodeFlags Flags;
        public int Parent;
        public int Left;
        public int Right;

        public bool IsRoot => (Flags & BTNodeFlags.NotRoot) == 0;
        public bool IsParallel => (Flags & BTNodeFlags.Parallel) != 0;
        public bool IsSelectorOrParallel => (Flags & (BTNodeFlags.Selector | BTNodeFlags.Parallel)) != 0;
        public bool IsSequenceOrParallel => (Flags & (BTNodeFlags.Sequence | BTNodeFlags.Parallel)) != 0;

        public void SetPerent(int parent, BTNodeFlags flags)
        {
            Parent = parent;
            Flags |= flags;
        }
    }


    public struct BTArgument<T>
    {
        internal BTNodeFunc<T> Func;
        internal int NodeIndex;

        public static implicit operator BTArgument<T>(BTNodeFunc<T> func)
        {
            return new BTArgument<T>()
            {
                Func = func,
            };
        }

        public static implicit operator BTArgument<T>(int nodeIndex)
        {
            return new BTArgument<T>()
            {
                NodeIndex = nodeIndex,
            };
        }
    }


    public class BehaviorTree<T>
    {
        public static BTNodeFunc<T> ProcessPlug = (ref T payload) => BTResult.Pending;

        private int _nextNodeID = 0;
        private int _nextDelegateID = 0;
        private readonly BTNode[] _nodes;
        private readonly BTNodeFunc<T>[] _delegates;

        public BehaviorTree(int maxNodes = 128, int maxDelegates = 128)
        {
            _nodes = new BTNode[maxNodes];
            _delegates = new BTNodeFunc<T>[maxDelegates];
        }

        public BTResult Process(ref BTState state, ref T payload)
        {
            int index = state.NodeIndex;
            bool isRight = state.IsRight;
            BTResult result = BTResult.Error;
            bool isFallback = false;
            do
            {
                ref BTNode node = ref _nodes[index];

                if (isFallback)
                {
                    isFallback = false;
                }
                else
                {
                    (int nextIndex, bool isFunc) = isRight ? (node.Right, (node.Flags & BTNodeFlags.RighFunc) != 0) : (node.Left, (node.Flags & BTNodeFlags.LeftFunc) != 0);
                    if (isFunc)
                    {
                        result = _delegates[nextIndex](ref payload);
                    }
                    else
                    {
                        isRight = false;
                        index = nextIndex;
                        continue;
                    }
                }

                switch (result)
                {
                    case BTResult.Success:
                        // Sequence и конец или Selector и удача
                        //bool isSlector = (node._flags & NodeFlags.Selector) != 0;
                        if (isRight || node.IsSelectorOrParallel)
                        {
                            result = BTResult.Success;
                            isFallback = true;
                        }
                        else // Sequence и лево -> продолжаем
                            isRight = true;
                        break;
                    case BTResult.Fail:
                        // Selector и конец или Sequence и удача
                        if (isRight || node.IsSequenceOrParallel)
                        {
                            result = BTResult.Fail;
                            isFallback = true;
                        }
                        else
                            isRight = true;
                        break;
                    case BTResult.Pending:
                        if (node.IsParallel)
                        {
                            if (isRight)
                                isFallback = true;
                            else
                                isRight = true;
                        }
                        else
                        {
                            state.NodeIndex = index;
                            state.IsRight = isRight;
                            state.Result = result;
                            return result;
                        }
                        break;
                    default:
                    case BTResult.Error:
                        return BTResult.Error;
                }

                if (isFallback)
                {
                    if (node.IsRoot)
                    {
                        state.NodeIndex = index;
                        state.IsRight = false;
                        state.Result = result;
                        return result;
                    }

                    index = node.Parent;
                    isRight = (node.Flags & BTNodeFlags.IsRigh) != 0;
                }
            } while (true);
        }

        #region Building nodes

        private int RegisterDelegate(BTNodeFunc<T> func)
        {
            int id = _nextDelegateID++;
            _delegates[id] = func;
            return id;
        }

        private ref BTNode RegisterNode(out int nodeId)
        {
            nodeId = _nextNodeID++;
            return ref _nodes[nodeId];
        }

        private int CreateNode(BTNodeFlags nodeType, in BTArgument<T> leftNode, in BTArgument<T> rightNode)
        {
            ref BTNode node = ref RegisterNode(out int nodeId);
            node.Flags = nodeType;
            if (leftNode.Func == null)
            {
                int leftIndex = leftNode.NodeIndex;
                node.Left = leftIndex;
                _nodes[leftIndex].SetPerent(nodeId, BTNodeFlags.NotRoot | BTNodeFlags.IsLeft);
            }
            else
            {
                node.Flags |= BTNodeFlags.LeftFunc;
                node.Left = RegisterDelegate(leftNode.Func);
            }

            if (rightNode.Func == null)
            {
                int rightIndex = rightNode.NodeIndex;
                node.Right = rightIndex;
                _nodes[rightIndex].SetPerent(nodeId, BTNodeFlags.NotRoot | BTNodeFlags.IsRigh);
            }
            else
            {
                node.Flags |= BTNodeFlags.RighFunc;
                node.Right = RegisterDelegate(rightNode.Func);
            }

            return nodeId;
        }

        private int CreateNode(BTNodeFlags nodeType, BTArgument<T> n1, BTArgument<T> n2, BTArgument<T> n3)
        {
            return CreateNode(nodeType, n1, CreateNode(nodeType, n2, n3));
        }

        private int CreateNode(BTNodeFlags nodeType, BTArgument<T> n1, BTArgument<T> n2, BTArgument<T> n3, BTArgument<T> n4)
        {
            return CreateNode(nodeType, n1, CreateNode(nodeType, n2, CreateNode(nodeType, n3, n4)));
        }


        // Sequence
        // Неудача при первой неудачи

        public int Sequence(BTArgument<T> n1, BTArgument<T> n2)
        {
            return CreateNode(BTNodeFlags.Sequence, n1, n2);
        }

        public int Sequence(BTArgument<T> n1, BTArgument<T> n2, BTArgument<T> n3)
        {
            return CreateNode(BTNodeFlags.Sequence, n1, n2, n3);
        }

        public int Sequence(BTArgument<T> n1, BTArgument<T> n2, BTArgument<T> n3, BTArgument<T> n4)
        {
            return CreateNode(BTNodeFlags.Sequence, n1, n2, n3, n4);
        }

        // Selector
        // Удача при первой удачи

        public int Selector(BTArgument<T> n1, BTArgument<T> n2)
        {
            return CreateNode(BTNodeFlags.Selector, n1, n2);
        }

        public int Selector(BTArgument<T> n1, BTArgument<T> n2, BTArgument<T> n3)
        {
            return CreateNode(BTNodeFlags.Selector, n1, n2, n3);
        }

        public int Selector(BTArgument<T> n1, BTArgument<T> n2, BTArgument<T> n3, BTArgument<T> n4)
        {
            return CreateNode(BTNodeFlags.Selector, n1, n2, n3, n4);
        }

        // Parallel
        // Первая удача или неудача

        public int Parallel(BTArgument<T> n1, BTArgument<T> n2)
        {
            return CreateNode(BTNodeFlags.Parallel, n1, n2);
        }

        public int Parallel(BTArgument<T> n1, BTArgument<T> n2, BTArgument<T> n3)
        {
            return CreateNode(BTNodeFlags.Parallel, n1, n2, n3);
        }

        public int Parallel(BTArgument<T> n1, BTArgument<T> n2, BTArgument<T> n3, BTArgument<T> n4)
        {
            return CreateNode(BTNodeFlags.Parallel, n1, n2, n3, n4);
        }

        public int EndPoint(BTArgument<T> n1)
        {
            return Parallel(n1, ProcessPlug);
        }

        #endregion
    }
}
