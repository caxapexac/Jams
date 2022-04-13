using System.Collections.Generic;
using Creatures;
using UnityEngine;


public sealed class Side : MonoBehaviour
{
    [Header("Settings")]
    public bool Inverted;
    public float BuildNewBuildingChance = 0.2f;
    public float WanderingRange = 1f;
    public float BuildingRange = 2.5f;
    public Building HomeBuildingPrefab;
    public Building WorkBuildingPrefab;
    public Building RestBuildingPrefab;

    [Header("Read Only")]
    public List<Building> Buildings = new List<Building>();
    public List<Human> Humans = new List<Human>();
    public JobType CurrentJobType = JobType.Work;
    public Human CurrentBuildPlanner = null;
    public bool IsDay = false;
    public float TimeOfTheDay = 0;
    public bool IsLose = false;

    private World _world;


    public enum JobType
    {
        Work,
        Rest,
        Sleep
    }


    private void Start()
    {
        _world = GetComponentInParent<World>();
    }

    private void Update()
    {
        float angle = _world.Utc - Vector2.SignedAngle(transform.up, _world.transform.up);
        angle = (angle + 360) % 360;
        TimeOfTheDay = angle;
        IsDay = angle < 180;
        if (TimeOfTheDay > 10 && TimeOfTheDay < 120)
        {
            CurrentJobType = JobType.Work;
        }
        else if (TimeOfTheDay < 180)
        {
            CurrentJobType = JobType.Rest;
        }
        else if (TimeOfTheDay < 270)
        {
            CurrentJobType = JobType.Sleep;
        }
        else
        {
            CurrentJobType = JobType.Sleep;
        }
    }

    public void Register(Building building)
    {
        Buildings.Add(building);
    }

    public void Unregister(Building building)
    {
        Buildings.Remove(building);
    }

    public void Register(Human human)
    {
        Humans.Add(human);
    }

    public void Unregister(Human human)
    {
        Humans.Remove(human);
    }

    public void Lose()
    {
        if (IsLose)
            return;

        IsLose = true;
        foreach (Building building in Buildings)
        {
            Rigidbody2D rb = building.gameObject.AddComponent<Rigidbody2D>();
            if (Inverted)
                rb.gravityScale = -1;
        }

        foreach (Human human in Humans)
        {
            Rigidbody2D rb = human.gameObject.AddComponent<Rigidbody2D>();
            if (Inverted)
                rb.gravityScale = -1;
        }
    }
}
