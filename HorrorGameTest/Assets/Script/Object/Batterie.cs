using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class Batterie : InteractiveObj
{
    [SerializeField] private int numberOfBatterie = 1;

    public override void Interact(PlayerController player)
    {
        PlayerInteraction.Instance.AddBatterie(numberOfBatterie);
        Destroy(gameObject);
    }
}
