using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Settings : ScriptableObject
{
    public float JumpTime;
    public float JumpSpeed;
    public float MoveSpeed;
    public float MaxSpeed;
    
    public int Hp;
}
