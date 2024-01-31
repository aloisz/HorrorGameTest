using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class ObjGenPro : MonoBehaviour
{
    public List<GameObject> ObjData;

    [Space] 
    private int countIndex = 0;
    public float width;
    public float height;

    [Space] public List<objGenerator> objGenerators;

    [Button("Spawn Objects")]
    public void SpawnObjects()
    {
        countIndex = 0;
        for (int i = 0; i <= objGenerators.Count; i++)
        {
            int countDensity = 0;
            do
            {
                Vector3 startRayPos = new Vector3(Random.Range(-width, width), 300, Random.Range(-height, height));
    
                RaycastHit hit;
                if (Physics.Raycast(startRayPos, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity))
                {
                    if (hit.point.y >= objGenerators[countIndex].minMaxSpwanPos.x && hit.point.y <= objGenerators[countIndex].minMaxSpwanPos.y)
                    {
                        Debug.DrawRay(startRayPos, transform.TransformDirection(Vector3.down) * hit.distance, Color.green , .5f);
                        Debug.Log("Did Hit");
                        
                        // Transform
                        GameObject obj = Instantiate(objGenerators[countIndex].gameObject, hit.point, Quaternion.identity, transform);
                        obj.transform.localScale = 
                            new Vector3(1, Random.Range(objGenerators[countIndex].SizeOfObj.x, objGenerators[countIndex].SizeOfObj.y), 1);
                        
                        // Rotation
                        //Manage the inclination of the normals
                        Quaternion spawnRot = Quaternion.SlerpUnclamped(
                                Quaternion.FromToRotation(Vector3.up, hit.normal), 
                                Quaternion.identity, 
                                Random.Range(objGenerators[countIndex].minMaxObjectNormalRotation.x, objGenerators[countIndex].minMaxObjectNormalRotation.y));
                        obj.transform.rotation *= spawnRot * Quaternion.Euler(obj.transform.rotation.x, Random.Range(0,360), obj.transform.rotation.z);
                        
                        // Name Attribution
                        obj.name = objGenerators[countIndex].name;
                        
                        // Data
                        ObjData.Add(obj);
                        countDensity++;
                    }
                }
            } while (countDensity != objGenerators[countIndex].densityToSpawn);

            countIndex++;
        }
        
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
public class objGenerator
{
    public string name;
    public GameObject gameObject;
    public int densityToSpawn;
    [MinMaxSlider(0, 100)] public Vector2 minMaxSpwanPos;
    [MinMaxSlider(.5f, 2)] public Vector2 SizeOfObj;
    [MinMaxSlider(0, 1)]public Vector2 minMaxObjectNormalRotation;
}
