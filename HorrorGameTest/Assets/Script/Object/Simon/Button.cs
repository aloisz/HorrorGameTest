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
    private MeshRenderer meshrenderer;
    private void Start()
    {
        coll = GetComponent<Collider>();
        meshrenderer = GetComponent<MeshRenderer>();
        coll.enabled = false;
        
        Simon.instance.OnChangeCollider += ChangeCollider;
        basePos = transform.position;
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

    public void CallButton()
    {
        Debug.Log(buttonState);
        transform.DOMove(PressedButtonPosition, .25f).OnComplete((() => 
            transform.DOMove(basePos, 0.25f)));
    }
}
