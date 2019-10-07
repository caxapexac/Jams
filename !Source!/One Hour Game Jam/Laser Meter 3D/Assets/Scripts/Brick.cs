using System;
using UnityEngine;

public class Brick : MonoBehaviour
{
    public bool state;
    
    private void Start()
    {
        God.Count++;
    }

    private void Update()
    {
        
    }

    private void OnTriggerExit(Collider other)
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        God.Count--;
    }
}