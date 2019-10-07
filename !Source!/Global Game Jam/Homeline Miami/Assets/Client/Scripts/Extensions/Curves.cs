using UnityEngine;


namespace Client.Scripts.Algorithms.Legacy
{
    public static class Curves
    {
        public static double CosLerp(double a, double b, double x)
        {
            double t = (1 - Mathf.Cos((float)(x * 3.1415927))) * 0.5f;
            return a * (1 - t) + b * t;
        }

        /// <summary>
        /// </summary>
        /// <param name="v0">точка до a</param>
        /// <param name="v1">точка a </param>
        /// <param name="v2">точка b </param>
        /// <param name="v3">точка после b</param>
        /// <param name="x"></param>
        /// <returns></returns>
        public static float CubicLerp(float v0, float v1, float v2, float v3, float x)
        {
            float p = (v3 - v2) - (v0 - v1);
            float q = (v0 - v1) - p;
            float r = v2 - v0;
            float s = v1;

            //возможно 3,2 - степени
            return p * x * 3 + q * x * 2 + r * x + s;
        }


        private static float LinearLerp(float a, float b, float t)
        {
            return a + t * (b - a);
        }

        private static float HermiteCurve(float t)
        {
            return t * t * (3 - 2 * t);
        }

        private static float QunticCurve(float t)
        {
            return t * t * t * (t * (t * 6 - 15) + 10);
        }
    }
}