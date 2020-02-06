using System;
using System.Collections;
using UnityEngine;

namespace UnityStandardAssets.Utility
{
    [Serializable]
    public class FOVKick
    {
        public Camera CurrentCamera;// optional camera setup, if null the main camera will be used
        [HideInInspector] public float OriginalFov;// the original fov
        public float FOVIncrease = 3f;// the amount the field of view increases when going into a run
        public float TimeToIncrease = 1f;// the amount of time the field of view will increase over
        public float TimeToDecrease = 1f;// the amount of time the field of view will take to return to its original size
        public AnimationCurve IncreaseCurve;


        public void Setup(Camera camera)
        {
            if (CurrentCamera == null)
            {
                throw new Exception("FOVKick camera is null, please supply the camera to the constructor");
            }

            if (IncreaseCurve == null)
            {
                throw new Exception("FOVKick Increase curve is null, please define the curve for the field of view kicks");
            }
            CurrentCamera = camera;
            OriginalFov = camera.fieldOfView;
        }

        public void ChangeCamera(Camera camera)
        {
            CurrentCamera = camera;
        }


        public IEnumerator FOVKickUp()
        {
            float t = Mathf.Abs((CurrentCamera.fieldOfView - OriginalFov) / FOVIncrease);
            while (t < TimeToIncrease)
            {
                CurrentCamera.fieldOfView = OriginalFov + (IncreaseCurve.Evaluate(t / TimeToIncrease) * FOVIncrease);
                t += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
        }


        public IEnumerator FOVKickDown()
        {
            float t = Mathf.Abs((CurrentCamera.fieldOfView - OriginalFov) / FOVIncrease);
            while (t > 0)
            {
                CurrentCamera.fieldOfView = OriginalFov + (IncreaseCurve.Evaluate(t / TimeToDecrease) * FOVIncrease);
                t -= Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            //make sure that fov returns to the original size
            CurrentCamera.fieldOfView = OriginalFov;
        }
    }
}
