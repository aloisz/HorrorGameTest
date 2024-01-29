using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public static class Noise 
{
    public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight,int seed ,float noiseScale, int octaves, float persistance, float lacunarity, Vector2 OffSet)
    {
        float[,] noiseMap = new float[mapWidth, mapHeight];

        System.Random prng = new Random(seed);
        Vector2[] octaveOffSet = new Vector2[octaves];
        for (int i = 0; i < octaves; i++)
        {
            float offSetX = prng.Next(-100000, 100000) + OffSet.x;
            float offSetY = prng.Next(-100000, 100000) + OffSet.y;
            octaveOffSet[i] = new Vector2(offSetX, offSetY);
        }

        // security if its null
        if (noiseScale <= 0) noiseScale = 0.0001f;


        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        float halfWidht = mapWidth / 2;
        float halfHeight = mapHeight / 2;
        
        // Asign the perlin noise
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;
                
                for (int i = 0; i < octaves; i++)
                {
                    float sampleX = (x - halfWidht) / noiseScale * frequency + octaveOffSet[i].x; // the higher the frequency the higher the sample point 
                    float sampleY = (y - halfHeight) / noiseScale * frequency + octaveOffSet[i].y;

                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                    noiseHeight += perlinValue * amplitude;
                    amplitude *= persistance;
                    frequency *= lacunarity;
                }
                
                if(noiseHeight > maxNoiseHeight) maxNoiseHeight = noiseHeight;
                else if (noiseHeight < minNoiseHeight) minNoiseHeight = noiseHeight;
                
                noiseMap[x, y] = noiseHeight;
            }
        }

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);
            }
        }
        return noiseMap;
    }
}
