using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{
    public GameObject Platform;
    public float Delay;
    public float Tick;
    
    void Start()
    {
        Tick = Time.time;
    }

    void Update()
    {
        if (Time.time - Tick > Delay)
        {
            Instantiate(Platform, new Vector2((Random.value - 0.5f) * 30, (Random.value - 0.5f) * 30), Quaternion.identity);
            Tick = Time.time;
        }
    }
}
