using System;
using UnityEngine;


namespace UnityStandardAssets.Utility
{
    [Serializable]
    public class CurveControlledBob
    {
        public float HorizontalBobRange = 0.33f;
        public float VerticalBobRange = 0.33f;
        public AnimationCurve Bobcurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(0.5f, 1f),
                                                            new Keyframe(1f, 0f), new Keyframe(1.5f, -1f),
                                                            new Keyframe(2f, 0f)); // sin curve for head bob
        public float VerticaltoHorizontalRatio = 1f;

        private float cyclePositionX;
        private float cyclePositionY;
        private float bobBaseInterval;
        private Vector3 originalCameraPosition;
        private float time;


        public void Setup(Camera camera, float bobBaseInterval)
        {
            this.bobBaseInterval = bobBaseInterval;
            originalCameraPosition = camera.transform.localPosition;

            // get the length of the curve in time
            time = Bobcurve[Bobcurve.length - 1].time;
        }


        public Vector3 DoHeadBob(float speed)
        {
            float xPos = originalCameraPosition.x + (Bobcurve.Evaluate(cyclePositionX) * HorizontalBobRange);
            float yPos = originalCameraPosition.y + (Bobcurve.Evaluate(cyclePositionY) * VerticalBobRange);

            cyclePositionX += (speed * Time.deltaTime) / bobBaseInterval;
            cyclePositionY += ((speed * Time.deltaTime) / bobBaseInterval) * VerticaltoHorizontalRatio;

            if (cyclePositionX > time)
            {
                cyclePositionX = cyclePositionX - time;
            }
            if (cyclePositionY > time)
            {
                cyclePositionY = cyclePositionY - time;
            }

            return new Vector3(xPos, yPos, 0f);
        }
    }
}
