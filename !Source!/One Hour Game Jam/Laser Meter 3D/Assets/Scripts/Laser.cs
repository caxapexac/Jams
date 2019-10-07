using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public Vector2 Dir;
    public Color Color;
    public int Speed;
    public new Rigidbody rigidbody;

    private void Start()
    {
        rigidbody = GetComponentInChildren<Rigidbody>();
    }

    private void Update()
    {
        transform.Rotate(new Vector3(-Input.GetAxis("Vertical"),Input.GetAxis ("Horizontal")));
    }
}