using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TextureGenerator 
{

    public static Texture2D TextureFromColorMap(Color[] colorMap, int width, int height)
    {
        Texture2D texture2D = new Texture2D(width, height);
        texture2D.filterMode = FilterMode.Point;
        texture2D.wrapMode = TextureWrapMode.Clamp;
        texture2D.SetPixels(colorMap);
        texture2D.Apply();
        return texture2D;
    }

    public static Texture2D TextureFromHeightMap(float[,] heightMap)
    {
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);

        Color[] colorMap = new Color[width * height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                colorMap[y * width + x] = Color.Lerp(Color.black, Color.white, heightMap[x, y]); // Apply color to texture with noiseMap
            }
        }
        return TextureFromColorMap(colorMap, width, height);
    }
}
