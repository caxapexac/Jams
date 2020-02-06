using UnityEngine;
using System.Collections;

public class ObjectRotateClamp : MonoBehaviour
{
    public float RotateAmount;

    void Update()
    {
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, (Mathf.Sin(Time.realtimeSinceStartup) * RotateAmount) + transform.eulerAngles.y, transform.eulerAngles.z);
    }
}
