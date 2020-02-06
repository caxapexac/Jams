using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public DurabilityManager DM;
    // Start is called before the first frame update

    private void Start()
    {
        DM = GetComponentInParent<DurabilityManager>();
    }

    private void OnTriggerExit(Collider other)
    {
        CarComponent car = other.GetComponentInParent<CarComponent>();
        if (car != null)
        {
            DM.Break(1);
            car.DamageMade += 1;

            //Debug.Log(gameObject.name + " durability: " + durability);
        }
        Debug.Log(other.gameObject.name);
    }
}
