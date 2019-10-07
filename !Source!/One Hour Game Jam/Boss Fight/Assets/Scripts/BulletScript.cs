using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public string Parent;
    
    public float Speed;

    public Vector2 Rotation;

    public Camera _camera;

    private void Update()
    {
        transform.Translate(Rotation * Time.deltaTime * Speed);
    }

    private void OnBecameInvisible()
    {
        Rotation = new Vector2(-Rotation.x + Random.value - 0.5f, -Rotation.y + Random.value - 0.5f);
    }
}
