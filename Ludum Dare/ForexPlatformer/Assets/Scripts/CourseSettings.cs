using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CourseSettings : ScriptableObject
{
    public string Name;    
    [Range(-1, 1)]
    public float Direction;
    [Range(0, 1)]
    public float Delta;
    [Range(0, 1)]
    public float Chance;
    [Range(0, 100)]
    public float Difficulty;
    [Range(0, 1)]
    public float Delay;
}
