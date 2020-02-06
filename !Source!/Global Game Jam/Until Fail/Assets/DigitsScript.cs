using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DigitsScript : MonoBehaviour
{
    public float Speed;

    public float StartTime;

    public float EndTime;
    // Start is called before the first frame update
    void Start()
    {
        Speed = God.Instance.TextSpeed;
        EndTime = God.Instance.TextTime;
        StartTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - StartTime > EndTime)
        {
            Destroy(gameObject);
            return;
        }
        Camera camera = Camera.main;
        transform.position += Vector3.up * Time.deltaTime * Speed;
        transform.LookAt(transform.position + camera.transform.rotation * Vector3.forward, camera.transform.rotation * Vector3.up);
    }
}
