using System;
using MonoBehs;
using UnityEngine;

namespace Player
{
    public class RotationScript : MonoBehaviour
    {
        private void Update()
        {
            if (God.I.State == GameState.Playing)
            {
                transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(0, Mathf.PingPong(Time.time * 7, 140f) - 70, 0), 0.5f);
            }
            else
            {
                transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.identity, 0.3f);
            }
        }
    }
}