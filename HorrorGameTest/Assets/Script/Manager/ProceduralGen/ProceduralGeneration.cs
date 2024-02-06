using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ProceduralGeneration : MonoBehaviour
{
    public GameObject room;
    public GameObject bulletOfIce;

    [Space] 
    public int lenghtDungeon = 10;
    public int heightDungeon = 10;
    public int nbrOfRoom;
    [Space]
    public float gridOffset;
    public float gridOffsetHeight;
    private Room[,,] rooms;
    public static ProceduralGeneration instance;


    public void Awake()
    {
        instance = this;
        
    }

    public void Start()
    {
        rooms = new Room[lenghtDungeon, heightDungeon, lenghtDungeon];
        
        for (int x = 0; x < rooms.GetLength(0); x++)
        {
            for (int y = 0; y < rooms.GetLength(1); y++)
            {
                for (int z = 0; z < rooms.GetLength(2); z++)
                {
                    GameObject roomDungeon = Instantiate(room, new Vector3(x * gridOffset, y * gridOffsetHeight, z * gridOffset), Quaternion.identity, transform);
                    roomDungeon.name = "Room";

                    int random = Random.Range(0, 20);
                    if (random == 0)
                    {
                        GameObject bulletOfIce = Instantiate(this.bulletOfIce, new Vector3(x * gridOffset, 0, z * gridOffset), Quaternion.identity, transform);
                        bulletOfIce.transform.position += new Vector3(10, 1, 0);
                    }
                }
            }
        }
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
