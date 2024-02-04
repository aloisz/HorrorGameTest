using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;
using DG.Tweening;

public class Locker : Obj
{
    [SerializeField] private bool isLocked;
    [SerializeField] private bool isOccupied;

    [Space] 
    [SerializeField] private Vector3 insideLockerPoint; // position where the player is when inside the locker
    [SerializeField] private Vector3 outsideLockerPoint; // position where the player is when leaving the locker
    
    private float baseLookSpeed;
    [SerializeField]private float lookSpeed = 2f;
    
    private Collider coll;
    
    void Start()
    {
        coll = GetComponent<Collider>();
        insideLockerPoint = transform.position;
        outsideLockerPoint = transform.position + Vector3.forward;
    }

    public override void Interact(PlayerController player)
    {
        LockerLogic();
    }

    private void LockerLogic()
    {
        if (!isLocked && !isOccupied)
        {
            EnterTheLocker();
        }
        else
        {
            ExitTheLocker();
        }
    }

    private void EnterTheLocker()
    {
        //TODO Logic to enter the Locker
        baseLookSpeed = PlayerController.Instance.lookSpeed;
        PlayerController.Instance.lookSpeed = lookSpeed;
        
        isOccupied = true;
        PlayerController.Instance.ChangeState(PlayerState.Idle);
        PlayerController.Instance.characterController.enabled = false;
        PlayerController.Instance.canMove = false;
        
        PlayerController.Instance.transform.DOMove(insideLockerPoint, 1);
        PlayerController.Instance.transform.DORotate(transform.forward, 1);
    }

    private void ExitTheLocker()
    {
        //TODO Logic to exit the Locker
        PlayerController.Instance.lookSpeed = baseLookSpeed;
        
        PlayerController.Instance.transform.DOMove(outsideLockerPoint, 1).OnComplete((() => 
            OnCompleteExit()));
    }

    private void OnCompleteExit()
    {
        PlayerController.Instance.ChangeState(PlayerState.Idle);
        PlayerController.Instance.characterController.enabled = true;
        coll.enabled = true;
        isOccupied = false;
        PlayerController.Instance.canMove = true;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(insideLockerPoint, .5f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(outsideLockerPoint, .5f);
    }
}
