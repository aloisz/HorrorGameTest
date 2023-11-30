using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class Batterie : InteractiveObj
{
    [SerializeField] private int numberOfBatterie = 1;
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    public override void Interact(PlayerController player)
    {
        player.AddBatterie(numberOfBatterie);
        Destroy(gameObject);
    }
}
