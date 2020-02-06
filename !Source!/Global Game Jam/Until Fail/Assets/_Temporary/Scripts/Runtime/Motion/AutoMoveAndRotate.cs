using System;
using UnityEngine;

namespace UnityStandardAssets.Utility
{
    public class AutoMoveAndRotate : MonoBehaviour
    {
        public Vector3andSpace MoveUnitsPerSecond;
        public Vector3andSpace RotateDegreesPerSecond;
        public bool IgnoreTimescale;

        private float lastRealTime;


        private void Start()
        {
            lastRealTime = Time.realtimeSinceStartup;
        }


        // Update is called once per frame
        private void Update()
        {
            float deltaTime = Time.deltaTime;
            if (IgnoreTimescale)
            {
                deltaTime = (Time.realtimeSinceStartup - lastRealTime);
                lastRealTime = Time.realtimeSinceStartup;
            }
            transform.Translate(MoveUnitsPerSecond.value*deltaTime, MoveUnitsPerSecond.space);
            transform.Rotate(RotateDegreesPerSecond.value*deltaTime, MoveUnitsPerSecond.space);
        }


        [Serializable]
        public class Vector3andSpace
        {
            public Vector3 value;
            public Space space = Space.Self;
        }
    }
}
