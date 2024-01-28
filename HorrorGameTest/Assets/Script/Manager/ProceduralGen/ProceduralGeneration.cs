using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralGeneration : MonoBehaviour
{
    public GameObject room;
    public GameObject door;

    [Space] 
    public int lenghtDungeon = 10;
    public int nbrOfRoom;
    [Space]
    public float gridOffset;
    private Room[,] rooms;
    public static ProceduralGeneration instance;


    public void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        /*rooms = new Room[lenghtDungeon, lenghtDungeon];
        
        for (int x = 0; x < rooms.GetLength(0); x++)
        {
            for (int z = 0; z < rooms.GetLength(1); z++)
            {
                GameObject roomDungeon = Instantiate(room, new Vector3(x * gridOffset, 0, z * gridOffset), Quaternion.identity, transform);
                roomDungeon.name = "Room";
            }
        }*/
    }

    public void InstantiatingRooms(Dictionary<Direction, Vector3> posOfDoors)
    {
        if (posOfDoors == null) return;
        if(nbrOfRoom == lenghtDungeon) return;
        nbrOfRoom++;
        foreach (var Values in posOfDoors.Values)
        {
            GameObject roomDungeon = Instantiate(room, new Vector3(Values.x * gridOffset, 0, Values.z * gridOffset), Quaternion.identity, transform);
            roomDungeon.name = "Room";
        }
    }
    
}
