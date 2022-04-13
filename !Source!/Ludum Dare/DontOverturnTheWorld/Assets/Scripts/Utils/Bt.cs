// ----------------------------------------------------------------------------
// The MIT License
// LeoBT - Simple BehaviourTree framework https://github.com/Leopotam/bt
// Copyright (c) 2017-2019 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;


namespace Utils
{
    /// <summary>
    /// Behaviour tree main class.
    /// </summary>
    public sealed class Bt<T> where T : class, new()
    {
        public readonly BtSeq<T> Root = new BtSeq<T>();

        private readonly T _store;

        /// <summary>
        /// Creates new instance BehaviourTree class with custom store logic.
        /// </summary>
        /// <param name="store">Store logic instance.</param>
        public Bt(T store)
        {
#if DEBUG
            if (store == null)
            {
                throw new ArgumentNullException(nameof(store));
            }
#endif
            _store = store;
        }

        /// <summary>
        /// Runs logic of behaviour tree graph.
        /// </summary>
        public BtResult Run()
        {
            return Root.Run(_store);
        }
    }


    /// <summary>
    /// Result of bt-node execution.
    /// </summary>
    public enum BtResult
    {
        Success,
        Fail,
        Pending
    }


    /// <summary>
    /// Base class of all bt-nodes.
    /// </summary>
    public abstract class BtNode<T>
    {
        /// <summary>
        /// Process node logic.
        /// </summary>
        public abstract BtResult Run(T store);
    }


    /// <summary>
    /// Base class for all bt-container nodes.
    /// </summary>
    public abstract class BtGroup<T> : BtNode<T>
    {
        protected readonly List<BtNode<T>> _nodes = new List<BtNode<T>>(8);

        /// <summary>
        /// Adds new node with custom logic.
        /// </summary>
        /// <param name="cb">Logic callback.</param>
        public BtGroup<T> Then(Func<T, BtResult> cb)
        {
#if DEBUG
            if (cb == null)
            {
                throw new ArgumentNullException(nameof(cb));
            }
#endif
            Then(new BtAction<T>(cb));
            return this;
        }

        /// <summary>
        /// Adds new node to group.
        /// </summary>
        /// <param name="node">Node.</param>
        public BtGroup<T> Then(BtNode<T> node)
        {
#if DEBUG
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }
#endif
            _nodes.Add(node);
            return this;
        }

        /// <summary>
        /// Adds new Sequence-group node.
        /// </summary>
        public BtGroup<T> Seq()
        {
            var node = new BtSeq<T>();
            _nodes.Add(node);
            return node;
        }

        /// <summary>
        /// Adds new Parallel-group node.
        /// </summary>
        public BtGroup<T> Parallel()
        {
            var node = new BtParallel<T>();
            _nodes.Add(node);
            return node;
        }

        /// <summary>
        /// Adds new Select-group node.
        /// </summary>
        public BtGroup<T> Select()
        {
            var node = new BtSelect<T>();
            _nodes.Add(node);
            return node;
        }

        /// <summary>
        /// Adds new Conditional node with custom logic.
        /// </summary>
        /// <param name="cb">Condition.</param>
        public BtIfNode<T> When(Func<T, BtResult> cb)
        {
#if DEBUG
            if (cb == null)
            {
                throw new ArgumentNullException(nameof(cb));
            }
#endif
            var node = new BtIfNode<T>(cb);
            _nodes.Add(node);
            return node;
        }

        /// <summary>
        /// Adds new Conditional node with another node as logic.
        /// </summary>
        /// <param name="condition">Condition.</param>
        public BtIfNode<T> If(BtNode<T> condition)
        {
#if DEBUG
            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition));
            }
#endif
            var node = new BtIfNode<T>(condition);
            _nodes.Add(node);
            return node;
        }
    }


    /// <summary>
    /// Wrapper for simple logic callback.
    /// </summary>
    public sealed class BtAction<T> : BtNode<T>
    {
        private readonly Func<T, BtResult> _cb;

        /// <summary>
        /// Creates new instance of BtAction node.
        /// </summary>
        /// <param name="cb">Callback of custom logic.</param>
        public BtAction(Func<T, BtResult> cb)
        {
#if DEBUG
            if (cb == null)
            {
                throw new ArgumentNullException(nameof(cb));
            }
#endif
            _cb = cb;
        }

        public override BtResult Run(T store)
        {
#if DEBUG
            if (store == null)
            {
                throw new ArgumentNullException(nameof(store));
            }
#endif
            return _cb(store);
        }
    }


    /// <summary>
    /// Sequence container bt-node.
    /// </summary>
    public sealed class BtSeq<T> : BtGroup<T>
    {
        private int _currentChild;

        public override BtResult Run(T store)
        {
#if DEBUG
            if (store == null)
            {
                throw new ArgumentNullException(nameof(store));
            }
#endif
            var res = BtResult.Success;
            for (int i = _currentChild, iMax = _nodes.Count; i < iMax; i++, _currentChild++)
            {
                res = _nodes[i].Run(store);
                if (res != BtResult.Success)
                {
                    break;
                }
            }
            if (res != BtResult.Pending)
            {
                _currentChild = 0;
            }
            return res;
        }
    }


    /// <summary>
    /// Parallel container bt-node.
    /// </summary>
    public sealed class BtParallel<T> : BtGroup<T>
    {
        public override BtResult Run(T store)
        {
#if DEBUG
            if (store == null)
            {
                throw new ArgumentNullException(nameof(store));
            }
#endif
            var isPending = false;
            for (int i = 0, iMax = _nodes.Count; i < iMax; i++)
            {
                if (_nodes[i].Run(store) == BtResult.Pending)
                {
                    isPending = true;
                }
            }
            return isPending ? BtResult.Pending : BtResult.Success;
        }
    }


    /// <summary>
    /// Selector container bt-node.
    /// </summary>
    public sealed class BtSelect<T> : BtGroup<T>
    {
        private int _currentChild;

        public override BtResult Run(T store)
        {
#if DEBUG
            if (store == null)
            {
                throw new ArgumentNullException(nameof(store));
            }
#endif
            var res = BtResult.Success;
            for (int i = _currentChild, iMax = _nodes.Count; i < iMax; i++, _currentChild++)
            {
                res = _nodes[i].Run(store);
                if (res != BtResult.Fail)
                {
                    break;
                }
            }
            if (res != BtResult.Pending)
            {
                _currentChild = 0;
            }
            return res;
        }
    }


    /// <summary>
    /// Conditional bt-node.
    /// </summary>
    public sealed class BtIfNode<T> : BtNode<T>
    {
        private readonly BtNode<T> _condition;
        private BtNode<T> _onSuccess;

        /// <summary>
        /// Creates new instance of Conditional node.
        /// </summary>
        /// <param name="condition">Callback of custom logic for condition checking.</param>
        public BtIfNode(Func<T, BtResult> condition) : this(new BtAction<T>(condition))
        {
        }

        /// <summary>
        /// Creates new instance of Conditional node.
        /// </summary>
        /// <param name="condition">Condition node.</param>
        /// <param name="successNode">Node for processing on successful condition.</param>
        public BtIfNode(BtNode<T> condition, BtNode<T> successNode = null)
        {
#if DEBUG
            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition));
            }
#endif
            _condition = condition;
            _onSuccess = successNode;
        }

        /// <summary>
        /// Sets node for processing on successful condition.
        /// </summary>
        /// <param name="cb">Callback of custom logic.</param>
        public BtIfNode<T> Then(Func<T, BtResult> cb)
        {
#if DEBUG
            if (cb == null)
            {
                throw new ArgumentNullException(nameof(cb));
            }
#endif
            _onSuccess = new BtAction<T>(cb);
            return this;
        }

        /// <summary>
        /// Sets node for processing on successful condition.
        /// </summary>
        /// <param name="successNode">Node for processing on successful condition.</param>
        public BtIfNode<T> Then(BtNode<T> successNode)
        {
#if DEBUG
            if (successNode == null)
            {
                throw new ArgumentNullException(nameof(successNode));
            }
#endif
            _onSuccess = successNode;
            return this;
        }

        public override BtResult Run(T store)
        {
#if DEBUG
            if (store == null)
            {
                throw new ArgumentNullException(nameof(store));
            }
#endif
            var res = _condition.Run(store);
            if (res == BtResult.Success && _onSuccess != null)
            {
                return _onSuccess.Run(store);
            }
            return res;
        }
    }
}
