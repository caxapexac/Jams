using System;
using System.Collections.Generic;
using System.Linq;
using Common.Behaviors;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;


namespace Creatures
{
    /// <summary>
    /// Parralel
    ///   - EnsureAerial фейл если в воздухе human.IsInAir
    ///   - Enforce go out form flying building human.MustGoOut -> human.CurrentBuilding, transform.SetParent()
    ///   - Sequence
    ///     - Rewrite -> human.CurrentJob
    ///     - Selector
    ///       - Sequence # Work on existing capacity building
    ///         - Find house of human.CurrentJob -> found as METHOD argument and human.TargetBuilding, building.Reserved
    ///         - Sequence # Go building
    ///           - Exit current building -> METHOD human.CurrentBuilding, building.Count, building.Reserved
    ///           - Approach human.CurrentBuilding -> METHOD MOVE transform.localPosition + animation
    ///           - Enter human.TargetBuilding -> METHOD human.CurrentBuilding, transform.parent, building.Count
    ///         - Working waiting for human.Side.CurrentJob change
    ///       - Sequence # build new building
    ///         - Random fail chance Side.FreeHumansCount %%
    ///         - Random position !!! Physics raycast bottom +-0.3f -> human.TargetPosition
    ///         - Approach human.TargetPosition -> METHOD MOVE transform.localPosition + animation
    ///         - Check free human.TargetPosition !!! Physics raycast bottom +-0.3f
    ///         - Instantiate home for N seconds !!! VFX
    ///         - Wait N seconds
    ///         - Success
    ///       - Sequence # Wandering
    ///         - Random position near -> human.TargetPosition
    ///         - Approach human.TargetPosition -> METHOD MOVE transform.localPosition + animation
    ///         - Wait N seconds + animation
    ///
    /// </summary>
    public static class HumanBrain
    {
        public static BehaviorTree<Human> Tree;
        public static int RootNode;

        private static List<RaycastHit2D> _collidersCache = new List<RaycastHit2D>(64);

        static HumanBrain()
        {
            Tree = new BehaviorTree<Human>();
            RootNode = Tree.Selector(
                WorkOnExistingBuilding(),
                BuildNewBuilding(),
                WanderingNear()
            );


            /*
            RootNode = Tree.Sequence(
                new BTNodeFunc<Human>(GetJob),
                Tree.Parallel(
                    new BTNodeFunc<Human>(EnforceGoOutFromBuilding),
                    new BTNodeFunc<Human>(EnsureOnGround),
                    new BTNodeFunc<Human>(CheckJobChange),
                    Tree.Selector(
                        WorkOnExistingBuilding(),
                        BuildNewBuilding(),
                        WanderingNear()
                    )
                )
            );
            */
        }

        private static BTResult EnforceGoOutFromBuilding(ref Human human)
        {
            if (human.MustGoOut)
            {
                human.Exit();
                return BTResult.Success;
            }

            return BTResult.Pending;
        }

        private static BTResult EnsureOnGround(ref Human human)
        {
            if (human.IsInAir)
            {
                return BTResult.Fail;
            }

            return BTResult.Pending;
        }

        private static BTResult CheckJobChange(ref Human human)
        {
            if (human.CurrentJob != human.Side.CurrentJobType)
                return BTResult.Fail;

            return BTResult.Pending;
        }

        private static BTResult GetJob(ref Human human)
        {
            if (human.Side.CurrentBuildPlanner == human)
            {
                human.Side.CurrentBuildPlanner = null;
            }
            human.CurrentJob = human.Side.CurrentJobType;
            return BTResult.Success;
        }

        private static int WorkOnExistingBuilding()
        {
            return Tree.Sequence(
                new BTNodeFunc<Human>(ReserveNearestJobBuilding),
                GoToTargetBuilding(),
                new BTNodeFunc<Human>(WorkInsideBuilding)
            );
        }

        private static BTResult ReserveNearestJobBuilding(ref Human human)
        {
            Side.JobType job = human.CurrentJob;
            float position = human.transform.localPosition.x;
            Building nearestBuilding = human.Side.Buildings
                .Where(building => building.ReservedChildren.Count < building.Capacity)
                .Where(building => building.JobType == job)
                .OrderBy(building => Mathf.Abs(position - building.transform.localPosition.x))
                .FirstOrDefault();

            if (nearestBuilding != null)
            {
                human.Reserve(nearestBuilding);
                return BTResult.Success;
            }
            return BTResult.Fail;
        }

        private static int GoToTargetBuilding()
        {
            return Tree.Sequence(
                new BTNodeFunc<Human>(ExitFromCurrentBuilding),
                new BTNodeFunc<Human>(ApproachTargetBuilding),
                new BTNodeFunc<Human>(EnterTargetBuilding)
            );
        }

        private static BTResult ExitFromCurrentBuilding(ref Human human)
        {
            human.Exit();
            return BTResult.Success;
        }

        private static BTResult ApproachTargetBuilding(ref Human human)
        {
            if (human.TargetBuilding == null)
                return BTResult.Fail;

            human.MoveTowards(human.TargetBuilding.transform.localPosition.x);
            if (Vector3.Distance(human.transform.localPosition, human.TargetBuilding.transform.localPosition) <= 0.05f)
            {
                return BTResult.Success;
            }
            return BTResult.Pending;
        }

        private static BTResult EnterTargetBuilding(ref Human human)
        {
            human.Enter(human.TargetBuilding);
            return BTResult.Success;
        }

        private static BTResult WorkInsideBuilding(ref Human human)
        {
            while (human.CurrentJob == human.CurrentBuilding.JobType)
            {
                return BTResult.Pending;
            }
            return BTResult.Success;
        }

        private static int BuildNewBuilding()
        {
            return Tree.Sequence(
                new BTNodeFunc<Human>((ref Human human) =>
                {
                    Side.JobType job = human.CurrentJob;
                    if (Random.Range(0, 1f) < human.Side.BuildNewBuildingChance
                        || human.Side.Buildings.Find(building => building.JobType == job) == null)
                    {
                        if (human.Side.CurrentBuildPlanner == null)
                        {
                            human.Side.CurrentBuildPlanner = human;
                            return BTResult.Success;
                        }
                    }
                    return BTResult.Fail;
                }),
                new BTNodeFunc<Human>(PreparePlaceForNewBuilding),
                new BTNodeFunc<Human>(MoveToTargetPosition),
                Tree.Sequence(
                    new BTNodeFunc<Human>(InstantiateBuilding),
                    WaitDelay(1f, BTResult.Success) // TODO build animation
                )
            );
        }

        private static BTResult PreparePlaceForNewBuilding(ref Human human)
        {
            float randomPosition = human.transform.localPosition.x + Random.Range(-1f, 1f) * human.Side.BuildingRange;
            human.TargetPosition = Mathf.Clamp(randomPosition, -human.Earth.Radius, human.Earth.Radius);
            float minDistance = 1f;
            foreach (Building existingBuilding in human.Side.Buildings)
            {
                if (Vector3.Distance(existingBuilding.transform.localPosition, new Vector3(human.TargetPosition, 0, 0)) < minDistance)
                    return BTResult.Fail;
            }
            human.TargetPosition = randomPosition;
            return BTResult.Success;
        }

        private static BTResult MoveToTargetPosition(ref Human human)
        {
            if (Mathf.Abs(human.transform.localPosition.x - human.TargetPosition) <= 0.05f)
            {
                return BTResult.Success;
            }
            human.MoveTowards(human.TargetPosition);
            return BTResult.Pending;
        }

        private static BTResult InstantiateBuilding(ref Human human)
        {
            float minDistance = 1f;
            foreach (Building existingBuilding in human.Side.Buildings)
            {
                if (Vector3.Distance(existingBuilding.transform.localPosition, new Vector3(human.TargetPosition, 0, 0)) < minDistance)
                    return BTResult.Fail;
            }

            Building prefab;
            switch (human.CurrentJob)
            {
                case Side.JobType.Work:
                    prefab = human.Side.WorkBuildingPrefab;
                    break;
                case Side.JobType.Rest:
                    prefab = human.Side.RestBuildingPrefab;
                    break;
                case Side.JobType.Sleep:
                    prefab = human.Side.HomeBuildingPrefab;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            Building building = Object.Instantiate(prefab, human.Side.transform);
            building.transform.localPosition = new Vector3(human.TargetPosition, 0, 0);
            return BTResult.Success;
        }


        private static int WaitDelay(float time, BTResult timeout)
        {
            return Tree.Sequence(
                new BTNodeFunc<Human>((ref Human human) =>
                {
                    human.StartTime = Time.time;
                    return BTResult.Success;
                }),
                new BTNodeFunc<Human>((ref Human human) =>
                {
                    if (Time.time > human.StartTime + time)
                    {
                        return timeout;
                    }
                    return BTResult.Pending;
                })
            );
        }

        private static int WanderingNear()
        {
            return WanderingForTime(1f, BTResult.Success);
        }

        private static int WanderingForTime(float time, BTResult timeout)
        {
            return InfiniteWander();

            // return Tree.Parallel(
            //     InfiniteWander(),
            //     WaitDelay(time, BTResult.Success)
            // );
        }

        private static int InfiniteWander()
        {
            return Tree.Sequence(
                new BTNodeFunc<Human>((ref Human human) =>
                {
                    float randomPosition = human.transform.localPosition.x + Random.Range(-1f, 1f) * human.Side.WanderingRange;
                    human.TargetPosition = Mathf.Clamp(randomPosition, -human.Earth.Radius, human.Earth.Radius);

                    // FIXME
                    human.StartTime = Time.time;

                    return BTResult.Success;
                }),
                new BTNodeFunc<Human>((ref Human human) =>
                {
                    // FIXME
                    if (Time.time > human.StartTime + 1f)
                    {
                        return BTResult.Success;
                    }

                    if (Mathf.Abs(human.transform.localPosition.x - human.TargetPosition) <= 0.05f)
                    {
                        float randomPosition = human.transform.localPosition.x + Random.Range(-1f, 1f) * human.Side.WanderingRange;
                        human.TargetPosition = Mathf.Clamp(randomPosition, -human.Earth.Radius, human.Earth.Radius);
                    }
                    else
                    {
                        human.MoveTowards(human.TargetPosition);
                    }
                    return BTResult.Pending;
                })
            );
        }

        public static void UpdateBrain(Human human)
        {
            bool reset = false;

            reset |= BTResult.Pending != EnforceGoOutFromBuilding(ref human);
            if (!reset)
                reset |= BTResult.Pending != EnsureOnGround(ref human);
            if (!reset)
                reset |= BTResult.Pending != CheckJobChange(ref human);
            if (!reset)
                reset |= BTResult.Pending != Tree.Process(ref human.State, ref human);
            if (reset)
            {
                GetJob(ref human);
                human.State = new BTState(RootNode);
            }
            else
            {
            }

            // switch (result)
            // {
            //     case BTResult.Success:
            //         break;
            //     case BTResult.Fail:
            //         break;
            //     case BTResult.Process:
            //         break;
            //     case BTResult.Error:
            //         break;
            //     default:
            //         throw new ArgumentOutOfRangeException();
            // }
        }
    }
}
