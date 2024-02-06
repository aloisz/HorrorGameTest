using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class Room : MonoBehaviour
{
    public int numbersOfDoors;
    [Space]
    private  Vector3[] doorsPosition;
    void Start()
    {
        doorsPosition = new Vector3[numbersOfDoors];
        doorsPosition[0] = new Vector3( transform.position.x + 10, 0, transform.position.z);
        doorsPosition[1] = new Vector3( transform.position.x, 0, transform.position.z + 10);
        doorsPosition[2] = new Vector3( transform.position.x - 10, 0, transform.position.z);
        doorsPosition[3] = new Vector3( transform.position.x, 0, transform.position.z - 10);
    }
    
    void Update()
    {
        
    }

    void OnDrawGizmos()
    {
        Handles.color = Color.red;
        
        foreach (var i in doorsPosition)
        {
            Handles.DrawWireArc(i, transform.up, -transform.right, 360, 2);
        }
        
    }
}

public enum Direction
{
    North,
    South,
    East,
    West
}


