using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public abstract class InteractiveObj : MonoBehaviour
{
    public abstract void Interact(PlayerController player);
}
