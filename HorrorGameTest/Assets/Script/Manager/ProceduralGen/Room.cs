using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class Room : MonoBehaviour
{
    
    public int numbersOfDoors;
    public Dictionary<Direction, Vector3> positionOfDoors = new Dictionary<Direction, Vector3>();
    public float posOfDoors = 2;
    void Start()
    {
        int temp = numbersOfDoors;
        numbersOfDoors = Random.Range(0, numbersOfDoors);
        
        positionOfDoors.Add(Direction.North, transform.position + new Vector3(0,0,posOfDoors));
        positionOfDoors.Add(Direction.South, transform.position - new Vector3(0,0,posOfDoors));
        positionOfDoors.Add(Direction.East, transform.position + new Vector3(posOfDoors,0,0));
        positionOfDoors.Add(Direction.West, transform.position - new Vector3(posOfDoors,0,0));

        if (numbersOfDoors == 0)
        {
            positionOfDoors.Clear();
        }
        else
        {
            for (int i = 0; i < temp - numbersOfDoors; i++)
            {
                positionOfDoors.Remove((Direction)Enum.ToObject(typeof(Direction) , Random.Range(0, 3)));
            }

            /*for (int i = 0; i < numbersOfDoors; i++)
            {
                Instantiate(ProceduralGeneration.instance.room, pos, Quaternion.identity, transform);
            }*/
            
            StartCoroutine(wait());
        }
    }
    
    IEnumerator wait()
    {
        yield return new WaitForSeconds(1);
        ProceduralGeneration.instance.InstantiatingRooms(positionOfDoors);
    }
    
    void Update()
    {
        
    }

    void OnDrawGizmos()
    {
        Handles.color = Color.red;
        
        foreach (var i in positionOfDoors)
        {
            Handles.DrawWireArc(i.Value, transform.up, -transform.right, 360, 2);
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


