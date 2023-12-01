using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using SimonGame;
using UnityEngine;
using DG.Tweening;
public class Button : InteractiveObj
{
    public SimonState buttonState;

    private Vector3 basePos;
    [SerializeField] private Vector3 PressedButtonPosition;
    private Collider coll;
    private void Start()
    {
        coll = GetComponent<Collider>();
        coll.enabled = false;
        Simon.instance.OnPlaySimon += ChangeCollider;
        basePos = transform.localPosition;
        PressedButtonPosition = basePos + new Vector3(0,-.2f,0);
    }

    public override void Interact(PlayerController player)
    {
        if (Simon.instance.canPlayTheSimon)
        {
            Simon.instance.PressButton(buttonState);
            transform.DOMove(PressedButtonPosition, .25f).OnComplete((() => 
                transform.DOMove(basePos, 0.25f)));
        }
    }

    private void ChangeCollider()
    {
        coll.enabled = Simon.instance.canPlayTheSimon;
    }
}
