using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class Obj : InteractiveObj
{
    public string name;
    public bool hasBeenInteractedWith;

    public override void Interact(PlayerController player)
    {
        throw new System.NotImplementedException();
    }
}