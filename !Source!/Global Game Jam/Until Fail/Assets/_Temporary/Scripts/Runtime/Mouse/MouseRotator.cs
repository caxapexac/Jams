using System;
using UnityEngine;

namespace UnityStandardAssets.Utility
{
    public class MouseRotator : MonoBehaviour
    {
        // A mouselook behaviour with constraints which operate relative to
        // this gameobject's initial rotation.
        // Only rotates around local X and Y.
        // Works in local coordinates, so if this object is parented
        // to another moving gameobject, its local constraints will
        // operate correctly
        // (Think: looking out the side window of a car, or a gun turret
        // on a moving spaceship with a limited angular range)
        // to have no constraints on an axis, set the rotationRange to 360 or greater.
        public Vector2 RotationRange = new Vector3 (70, 70);
        public float RotationSpeed = 10;
        public float DampingTime = 0.2f;
        public bool AutoZeroVerticalOnMobile = true;
        public bool AutoZeroHorizontalOnMobile = false;
        public bool Relative = true;

        private Vector3 targetAngles;
        private Vector3 followAngles;
        private Vector3 followVelocity;
        private Quaternion originalRotation;

        private void Start ()
        {
            originalRotation = transform.localRotation;
        }

        private void Update ()
        {
            // we make initial calculations from the original local rotation
            transform.localRotation = originalRotation;

            // read input from mouse or mobile controls
            float inputH;
            float inputV;
            if (Relative) {
                inputH = Input.GetAxis ("Mouse X");
                inputV = Input.GetAxis ("Mouse Y");

                // wrap values to avoid springing quickly the wrong way from positive to negative
                if (targetAngles.y > 180)
                {
                    targetAngles.y -= 360;
                    followAngles.y -= 360;
                }
                if (targetAngles.x > 180)
                {
                    targetAngles.x -= 360;
                    followAngles.x -= 360;
                }
                if (targetAngles.y < -180)
                {
                    targetAngles.y += 360;
                    followAngles.y += 360;
                }
                if (targetAngles.x < -180)
                {
                    targetAngles.x += 360;
                    followAngles.x += 360;
                }

                // with mouse input, we have direct control with no springback required.
                targetAngles.y += inputH * RotationSpeed;
                targetAngles.x += inputV * RotationSpeed;

                // clamp values to allowed range
                targetAngles.y = Mathf.Clamp (targetAngles.y, -RotationRange.y * 0.5f, RotationRange.y * 0.5f);
                targetAngles.x = Mathf.Clamp (targetAngles.x, -RotationRange.x * 0.5f, RotationRange.x * 0.5f);
            }
            else
            {
                inputH = Input.mousePosition.x;
                inputV = Input.mousePosition.y;

                // set values to allowed range
                targetAngles.y = Mathf.Lerp (-RotationRange.y * 0.5f, RotationRange.y * 0.5f, inputH / Screen.width);
                targetAngles.x = Mathf.Lerp (-RotationRange.x * 0.5f, RotationRange.x * 0.5f, inputV / Screen.height);
            }

            // smoothly interpolate current values to target angles
            followAngles = Vector3.SmoothDamp (followAngles, targetAngles, ref followVelocity, DampingTime);

            // update the actual gameobject's rotation
            transform.localRotation = originalRotation * Quaternion.Euler (-followAngles.x, followAngles.y, 0);
        }
    }
}