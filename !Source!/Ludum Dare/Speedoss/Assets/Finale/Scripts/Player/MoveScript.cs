using System;
using MonoBehs;
using UnityEngine;

namespace Player
{
    public class MoveScript : MonoBehaviour
    {
        public float Speed;
        
        private void Start()
        {
            Speed = 5;
        }

        private void Update()
        {
            if (God.I.State == GameState.Walk || God.I.State == GameState.Playing)
            {
                Speed += 2 * Time.deltaTime;
                transform.Translate(Speed * Time.deltaTime * Vector3.right);
            }
        }
    }
}