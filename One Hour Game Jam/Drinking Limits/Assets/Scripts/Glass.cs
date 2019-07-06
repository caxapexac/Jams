using System;
using UnityEngine;


public class Glass : MonoBehaviour
{
    public Transform Mask;
    public SpriteRenderer Fill;
    public float Min;
    public float Value;
    public float Max;
    public float Alco;
    public float Speed;

    private Vector3 _minscale = new Vector3(1.27f, 0, 1);
    private Vector3 _maxscale = new Vector3(1.27f, 2.5f, 1);
    private void Start()
    {
        
    }

    private void Update()
    {
        if (Value < Min)
        {
            God.Drunk -= Time.deltaTime * Alco;
            Fill.color = Color.red;
        }
        else if (Value > Max)
        {
            God.Drunk -= Time.deltaTime * 10 * Alco;
            Value -= Time.deltaTime * Speed;
            Fill.color = Color.red;
        }
        else
        {
            Fill.color = Color.Lerp(Color.cyan, Color.blue, Value);
            God.Drunk += Time.deltaTime * Alco;
            Value -= Time.deltaTime * Speed;
        }
        Mask.localScale = Vector3.Lerp(Mask.localScale, Vector3.Lerp(_minscale, _maxscale, Value), 0.1f);

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Value += Alco;
        Destroy(other.gameObject);
    }
}