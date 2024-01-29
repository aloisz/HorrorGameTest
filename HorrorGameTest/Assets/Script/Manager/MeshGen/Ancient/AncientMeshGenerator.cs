using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Video;
using Random = UnityEngine.Random;


[RequireComponent(typeof(MeshFilter))]
public class AncientMeshGenerator : MonoBehaviour
{
    private Mesh mesh;

    private Vector3[] vertices;
    private int[] triangles;
    private Vector2[] uv;

    public int xSize = 20;
    public int zSize = 20;
    public float offSet = 2;
    
    [Space]
    public float TerrainModificator = 2;

    [MinMaxSlider(0, 0.08f)] public Vector2 seed;
    
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        CreateShape();
        UpdateMesh();
    }

    [Button("Create Mesh")]
    private void CreateMesh()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        CreateShape();
        UpdateMesh();
    }

    private void CreateShape()
    {
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];

        int i = 0;
        for (int z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                float y = Mathf.PerlinNoise(x * Random.Range(seed.x, seed.y), z * Random.Range(seed.x, seed.y)) * TerrainModificator;
                vertices[i] = new Vector3(x * offSet, y * offSet, z * offSet);
                i++;
            }
        }

        triangles = new int[xSize * zSize * 6];
        int vert = 0;
        int tris = 0;

        for (int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {
                triangles[tris + 0] = vert +0;
                triangles[tris + 1] = vert +xSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize + 1;
                triangles[tris + 5] = vert + xSize + 2;

                vert++;
                tris += 6;
            }

            vert++;
        }
        
    }
    private void UpdateMesh()
    {
        if(Application.isPlaying) mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uv;
        
        mesh.RecalculateNormals();
        mesh.RecalculateBounds(); 
        DestroyImmediate(transform.GetComponent<MeshCollider>());
        transform.AddComponent<MeshCollider>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if(vertices == null) return;
        foreach (var pos in vertices)
        {
            Gizmos.DrawSphere(pos, .1f);
        }
    }
}
