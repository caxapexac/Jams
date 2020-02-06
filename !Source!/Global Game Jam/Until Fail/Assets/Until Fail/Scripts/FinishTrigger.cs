using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        CarComponent car = other.GetComponentInParent<CarComponent>();
        if (car)
        {
            car.Finish();
        }
    }
}
