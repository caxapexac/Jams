using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class ExchangeScript : MonoBehaviour
{
    public CourseSettings Course;

    public LineRenderer Line;
    public ParticleSystem Particles;
    public TextMeshProUGUI Text;
    public SpriteRenderer Sprite;
    public Vector2 MinVector2;
    public Vector2 MaxVector2;
    public float Current;
    public float Tick;
    public float Obuse;

    private void Start()
    {
        Obuse = 0;
        Current = Mathf.Clamp01(Random.value + Course.Direction);
        MinVector2 = Line.GetPosition(0);
        MaxVector2 = Line.GetPosition(Line.positionCount - 1);
        for (int i = 0; i < Line.positionCount; i++)
        {
            Line.SetPosition(i,
                new Vector2(Mathf.Lerp(MinVector2.x, MaxVector2.x, (float)i / Line.positionCount),
                    Mathf.Lerp(MinVector2.y, MaxVector2.y, Current)));
            Current = Mathf.Clamp01(Current + (Random.value - 0.5f + Course.Direction) * Course.Delta);
            if (Random.value < Course.Chance)
            {
                Current = Random.value;
            }
        }
        Text.text = Course.Name;
        Tick = Time.time;
    }

    private void Update()
    {
        Obuse = Mathf.Max(Obuse - Time.deltaTime / 10f, 0);
        if (Time.time - Tick + Random.value * 0.1f > Course.Delay)
        {
            for (int i = 0; i < Line.positionCount - 1; i++)
            {
                Line.SetPosition(i, new Vector2(Line.GetPosition(i).x, Line.GetPosition(i + 1).y));
            }
            if (Random.value < Course.Chance)
            {
                Current = Random.value;
            }
            Line.SetPosition(Line.positionCount - 1,
                new Vector2(MaxVector2.x,
                    Mathf.Lerp(MinVector2.y, MaxVector2.y,
                        Mathf.Clamp01(Current + (Random.value - 0.5f + Course.Direction - Obuse / 30f) * Course.Delta))));
            float average = 0;
            for (int i = 0; i < Line.positionCount / 4; i++)
            {
                average += Line.GetPosition(Line.positionCount - i - 2).y;
            }
            if (Line.GetPosition(Line.positionCount - 1).y > average / (Line.positionCount / 4))
            {
                Sprite.color = Color.green;
                Sprite.flipY = false;
            }
            else
            {
                Sprite.color = Color.red;
                Sprite.flipY = true;
            }
            Tick = Time.time;
        }
    }
}