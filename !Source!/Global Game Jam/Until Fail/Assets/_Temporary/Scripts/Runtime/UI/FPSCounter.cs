using System;
using UnityEngine;
using UnityEngine.UI;

namespace UnityStandardAssets.Utility
{
    [RequireComponent(typeof(Text))]
    public class FPSCounter : MonoBehaviour
    {
        const float FpsMeasurePeriod = 0.5f;
        const string Display = "{0} FPS";

        private int fpsAccumulator = 0;
        private float fpsNextPeriod = 0;
        private int currentFps;
        private Text textUi;


        private void Start()
        {
            fpsNextPeriod = Time.realtimeSinceStartup + FpsMeasurePeriod;
            textUi = GetComponent<Text>();
        }


        private void Update()
        {
            // measure average frames per second
            fpsAccumulator++;
            if (Time.realtimeSinceStartup > fpsNextPeriod)
            {
                currentFps = (int)(fpsAccumulator / FpsMeasurePeriod);
                fpsAccumulator = 0;
                fpsNextPeriod += FpsMeasurePeriod;
                textUi.text = string.Format(Display, currentFps);
            }
        }
    }
}
