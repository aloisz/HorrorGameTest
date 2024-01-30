using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class obj : MonoBehaviour
{
    [SerializeField] private MapGenerator mapGenerator;
    public GameObject objToSpawn;
    public List<GameObject> ObjData;
    public float minDist;
    public float maxDist;
    [MinMaxSlider(.5f, 2)] public Vector2 SizeOfObj;

    [Space] 
    public int densityToSpawn;
    public float width;
    public float height;

    [Space] public List<objListGenerator> objListGenerators;
    
    [Button("Spawn Objects")]
    public void SpawnObjects()
    {
        int count = 0;
        do
        {
            Vector3 startRayPos = new Vector3(Random.Range(-width, width), 300, Random.Range(-height, height));
        
            RaycastHit hit;
            if (Physics.Raycast(startRayPos, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity))
            {
                if (hit.point.y >= minDist && hit.point.y <= maxDist)
                {
                    Debug.DrawRay(startRayPos, transform.TransformDirection(Vector3.down) * hit.distance, Color.green , .5f);
                    Debug.Log("Did Hit");
                    GameObject obj = Instantiate(objToSpawn, hit.point, Quaternion.identity, transform);
                    obj.transform.localScale = new Vector3(1, Random.Range(SizeOfObj.x, SizeOfObj.y), 1);
                    obj.transform.rotation *= Quaternion.Euler(0, Random.Range(0,360), 0);
                    ObjData.Add(obj);
                    count++;
                }
                else
                {
                    Debug.DrawRay(startRayPos, transform.TransformDirection(Vector3.down) * hit.distance, Color.red , .5f);
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

[Serializable]
public class objListGenerator
{
    public objGenerator objGenerators;
}

[Serializable]
public class objGenerator
{
    public string name;
    public GameObject gameObject;
    public int densityToSpawn;
    [MinMaxSlider(0, 100)] public Vector2 minMaxSpwanPos;
    [MinMaxSlider(.5f, 2)] public Vector2 SizeOfObj;
}
