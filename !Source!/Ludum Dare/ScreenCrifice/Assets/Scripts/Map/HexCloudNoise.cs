using System;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Misc
{
    [Obsolete]
    public static class HexCloudNoise
    {
        public static int[,] dirs =
        {
            {0, 0},
            {1, 0},
            {-1, 1},
            {0, 1},
            {0, -1},
            {1, -1},
            {-1, 0}
        };

        private static float QunticCurve(float t)
        {
            //кто придумал этот пиздец?)
            return t * t * t * (t * (t * 6 - 15) + 10);
        }
        
        private static float[,] WhiteNoise(int size)
        {
            float[,] whiteNoise = new float[size, size];
            for (int i = 0; i < size; i++)
            {
                for (int k = 0; k < size; k++)
                {
                    float vector = Random.value;
                    whiteNoise[i, k] = vector;
                }
            }

            return whiteNoise;
        }

        public static float[,] Get(int saeed, int radius, int octaves)
        {
            Random.InitState(saeed);
            float[][,] noise = new float[octaves][,];
            for (int i = 0; i < octaves; i++)
            {
                noise[i] = WhiteNoise(radius / (i + 1));
            }

            float[,] hexNoise = new float[radius, radius];
            float amplitude = 1f;
            float max = 0;
            for (int d = 0; d < octaves; d++)
            {
                amplitude *= 0.5f;
                max += amplitude;
                for (int i = 0; i < radius; i++)
                {
                    for (int k = 0; k < radius; k++)
                    {
                        int dir = Mathf.Abs((-2 * i + k) % 7);
                        //Debug.Log(dir);
                        int x = i + dirs[dir, 0];
                        int y = k + dirs[dir, 1];
                        int a = (2 * x - y) / 7;
                        int b = (x + 3 * y) / 7;
                        //Debug.Log("d=" + d + " i=" + i + " k=" + k +" x=" + x +" y=" + y + " a=" + a + " b=" + b);
                        if (a < 0 || b < 0)
                        {
                            Debug.Log("i=" + i + " k=" + k +" x=" + x +" y=" + y + " a=" + a + " b=" + b);
                            int temp = a;
                            a = a + b + 1;
                            b = -temp;
                            //x = x + y
                            //y = -x
                        }

                        try
                        {
                            hexNoise[i, k] += noise[d][a, b] * amplitude;
                        }
                        catch (Exception e)
                        {
                            Debug.Log("d=" + d + " i=" + i + " k=" + k +" x=" + x +" y=" + y + " a=" + a + " b=" + b);
                            throw;
                        }
                        
                    }
                }
            }

            for (int i = 0; i < radius; i++)
            {
                for (int k = 0; k < radius; k++)
                {
                    hexNoise[i, k] /= max;
                }
            }

            return hexNoise;
        }
    }
}