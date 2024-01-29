using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

public class MapGenerator : MonoBehaviour
{
    public DrawMode drawMode;
    
    private const int mapChunkSize = 241;
    [Header("Details")]
    [Range(0,6)]
    public int levelOfDetail;
    public float noiseScale;

    [Header("Map Modificator")]
    public int octaves;
    [Range(0,1)]
    public float persistance;
    public float lacunarity;

    public int seed;
    public Vector2 offSet;

    public float meshHeightMultiplier;
    public AnimationCurve meshCurve;
    
    public TerrainType[] regions;

    public bool autoUpdate;


    private void Start()
    {
        GenerateMap();
    }

    [Button("GenerateMap")]
    public void GenerateMap()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(mapChunkSize, mapChunkSize, seed, noiseScale, octaves, persistance, lacunarity, offSet);
        
        Color[] coulourMap = new Color[mapChunkSize * mapChunkSize]; // colour of the map 
        for (int y = 0; y < mapChunkSize; y++)
        {
            for (int x = 0; x < mapChunkSize; x++)
            {
                float currentHeight = noiseMap[x,y];
                for (int i = 0; i < regions.Length; i++)
                {
                    if (currentHeight <= regions[i].height)
                    {
                        coulourMap[y * mapChunkSize + x] = regions[i].color; // applying color to the texture
                        break;
                    }
                }
            }
        }

        MapDisplay display = FindObjectOfType<MapDisplay>();

        switch (drawMode)
        {
            case DrawMode.NoiseMap:
                display.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap));
                break;
            case DrawMode.ColorMap:
                display.DrawTexture(TextureGenerator.TextureFromColorMap(coulourMap, mapChunkSize, mapChunkSize));
                break;
            case DrawMode.DrawMesh:
                display.DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap, meshHeightMultiplier, meshCurve, levelOfDetail), 
                    TextureGenerator.TextureFromColorMap(coulourMap, mapChunkSize, mapChunkSize));
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void OnValidate()
    {
        if (lacunarity < 1) lacunarity = 1;
        if (octaves < 0) octaves = 0;
    }
}

[System.Serializable]
public struct TerrainType
{
    public string name;
    public float height;
    public Color color;
}

public enum DrawMode
{
    NoiseMap,
    ColorMap,
    DrawMesh
}
