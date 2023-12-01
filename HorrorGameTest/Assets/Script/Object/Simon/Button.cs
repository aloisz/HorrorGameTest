using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using SimonGame;
using UnityEngine;

public class Button : InteractiveObj
{
    public SimonState buttonState;

    private Collider coll;
    private void Start()
    {
        coll = GetComponent<Collider>();
        coll.enabled = false;
        Simon.instance.OnPlaySimon += ChangeCollider;
    }

    public override void Interact(PlayerController player)
    {
        if (Simon.instance.canPlayTheSimon)
        {
            Simon.instance.PressButton(buttonState);
        }
    }

    private void ChangeCollider()
    {
        coll.enabled = Simon.instance.canPlayTheSimon;
    }
}
