using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Utility;

public class WaypointTrigger : MonoBehaviour
{
    public List<WaypointCircuit> Circuits;
    
    void Start()
    {
        Circuits = new List<WaypointCircuit>();
        foreach (WaypointCircuit circuit in GetComponentsInChildren<WaypointCircuit>())
        {
            Circuits.Add(circuit);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Trigger " + gameObject.name + " " + other.name);
        CarComponent car = other.GetComponentInParent<CarComponent>();
        if (car == null) return;
        car.ChangeWaypoint(Circuits[(int)Random.Range(0, Circuits.Count)]);
    }
}
