using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObjGenerator : MonoBehaviour
{
    [SerializeField] private MapGenerator mapGenerator;
    public GameObject objToSpawn;
    public List<GameObject> ObjData;
    public float minDist;

    [Space] 
    public int densityToSpawn;
    public float width;
    public float height;

    [Button("Spawn Objects")]
    public void SpawnObjects()
    {
        for (int ray = 0; ray < densityToSpawn; ray++)
        {
            
        }

        int count = 0;
        do
        {
            Vector3 startRayPos = new Vector3(Random.Range(-width, width), 300, Random.Range(-height, height));
        
            RaycastHit hit;
            if (Physics.Raycast(startRayPos, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity))
            {
                if (hit.point.y >= minDist)
                {
                    Debug.DrawRay(startRayPos, transform.TransformDirection(Vector3.down) * hit.distance, Color.green , 2);
                    Debug.Log("Did Hit");
                    GameObject tree = Instantiate(objToSpawn, hit.point, Quaternion.identity, transform);
                    ObjData.Add(tree);
                    count++;
                }
                else
                {
                    Debug.DrawRay(startRayPos, transform.TransformDirection(Vector3.down) * hit.distance, Color.red , 2);
                }
            }
        } while (count != densityToSpawn);
    }
    
    [Button("Clear")]
    public void ClearData()
    {
        foreach (var obj in ObjData)
        {
            DestroyImmediate(obj);
        }
        ObjData.Clear();
    }
}
