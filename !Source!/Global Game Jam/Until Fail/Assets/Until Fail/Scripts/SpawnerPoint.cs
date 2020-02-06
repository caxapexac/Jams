using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Utility;

public class SpawnerPoint : MonoBehaviour
{
    public enum SpawnerType
    {
        Goverment,
        Cilivian
    }

    public SpawnerType Type;
    public WaypointCircuit Waypoint;
    // Start is called before the first frame update
    void Start()
    {
        Waypoint = GetComponentInChildren<WaypointCircuit>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
