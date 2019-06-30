using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Spawner : MonoBehaviour
{
    public GameObject prefab;
    public float delay = 5;

    public float tick = 0;

    // Start is called before the first frame update
    void Start()
    { }

    // Update is called once per frame
    void Update()
    {
        tick += Time.deltaTime;
        if (tick > delay)
        {
            delay *= 0.9f;
            tick = 0;
            Instantiate(prefab, transform.position
                + new Vector3(Random.value, Random.value, Random.value), Random.rotation);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(prefab, transform.position
                + new Vector3(Random.value, Random.value, Random.value), Random.rotation);
            delay *= 0.9f;
        }
    }
}