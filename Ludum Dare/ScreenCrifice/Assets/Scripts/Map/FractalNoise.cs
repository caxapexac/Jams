using UnityEngine;
using Random = System.Random;

public class FractalNoise
{
    private static float[,] WhiteNoise(int saeed, int size)
    {
        Random random = new Random(saeed);
        float[,] noise = new float[size, size];

        for (int i = 0; i < size; i++)
        {
            for (int k = 0; k < size; k++)
            {
                noise[i, k] = (float) random.NextDouble() % 1;
            }
        }

        return noise;
    }

    private static float[,] SquareSmoothNoise(float[,] baseNoise, int size, int octave)
    {
        float[,] smoothNoise = new float[size, size];

        int period = 1 << octave; // calculates 2 ^ k
        float frequency = 1.0f / period;

        for (int i = 0; i < size; i++)
        {
            //calculate the horizontal sampling indices
            int sampleI0 = (i / period) * period;
            int sampleI1 = (sampleI0 + period) % size; //wrap around
            float horizontalBlend = (i - sampleI0) * frequency;

            for (int j = 0; j < size; j++)
            {
                //calculate the vertical sampling indices
                int sampleJ0 = (j / period) * period;
                int sampleJ1 = (sampleJ0 + period) % size; //wrap around
                float verticalBlend = (j - sampleJ0) * frequency;

                //blend the top two corners
                float top = Mathf.Lerp(baseNoise[sampleI0, sampleJ0], baseNoise[sampleI1, sampleJ0],
                    horizontalBlend);

                //blend the bottom two corners
                float bottom = Mathf.Lerp(baseNoise[sampleI0, sampleJ1], baseNoise[sampleI1, sampleJ1],
                    horizontalBlend);

                //final blend
                smoothNoise[i, j] = Mathf.Lerp(top, bottom, verticalBlend);
            }
        }

        return smoothNoise;
    }

    public static float[,] Get(int saeed, int size, int octaveCount, float persistance)
    {
        float[,] baseNoise = WhiteNoise(saeed, size);

        float[][,] smoothNoises = new float[octaveCount][,]; //an array of 2D arrays containing

        //generate smooth noise
        for (int i = 0; i < octaveCount; i++)
        {
            smoothNoises[i] = SquareSmoothNoise(baseNoise, size, i);
        }

        float[,] perlinNoise = new float[size, size];
        float amplitude = 1.0f;
        float totalAmplitude = 0.0f;

        //blend noise together
        for (int octave = octaveCount - 1; octave >= 0; octave--)
        {
            amplitude *= persistance;
            totalAmplitude += amplitude;

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    perlinNoise[i, j] += smoothNoises[octave][i, j] * amplitude;
                }
            }
        }

        //normalisation
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                perlinNoise[i, j] /= totalAmplitude;
            }
        }

        return perlinNoise;
    }

    private static float[,] HexSmoothNoise()
    {
        return null;
        //todo попробовать его
    }
}
