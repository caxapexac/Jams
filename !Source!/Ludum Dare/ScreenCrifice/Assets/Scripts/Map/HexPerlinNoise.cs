using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Misc
{
    [Obsolete]
    public class HexPerlinNoise
    {
        private struct SimpleVector
        {
            public SimpleVector(float x, float y)
            {
                X = x;
                Y = y;
            }

            public void Normalize()
            {
                float magnitude = Mathf.Sqrt(X * X + Y * Y);
                ;
                X /= magnitude;
                Y /= magnitude;
            }

            public float X;
            public float Y;
        }

        private static float QunticCurve(float t)
        {
            //кто придумал этот пиздец?)
            return t * t * t * (t * (t * 6 - 15) + 10);
        }

        private static float[,] Get(int saeed, int radius, int octaves)
        {
            Random.InitState(saeed);
            SimpleVector[][,] noise = new SimpleVector[octaves][,];
            for (int i = 0; i < octaves; i++)
            {
                noise[i] = WhiteNoise(radius / (i + 1) + 1);
            }




            return null;
        }

        private static SimpleVector[,] WhiteNoise(int size)
        {
            SimpleVector[,] whiteNoise = new SimpleVector[size, size];
            for (int i = 0; i < size; i++)
            {
                for (int k = 0; k < size; k++)
                {
                    SimpleVector vector = new SimpleVector(Random.value, Random.value);
                    vector.Normalize();
                    whiteNoise[i, k] = vector;
                }
            }

            return whiteNoise;
        }
    }
}