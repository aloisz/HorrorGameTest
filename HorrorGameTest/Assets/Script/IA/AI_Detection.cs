using System;
using System.Collections;
using System.Collections.Generic;
using AI;
using Player;
using SimonGame;
using UnityEngine;



public class AI_Detection : MonoBehaviour
{
    private SphereCollider sphereColl;
    [SerializeField] private float detectionRange = 5;
    private AI_HeadFollowing aiHeadFollowing;

    private void Start()
    {
        aiHeadFollowing = GetComponent<AI_HeadFollowing>();
        sphereColl = GetComponent<SphereCollider>();
        sphereColl.radius = detectionRange;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //aiHeadFollowing.WhatToLookAt(PlayerController.Instance.transform);
            Detected();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //aiHeadFollowing.WhatToLookAt(Simon.instance.transform);
            Undected();
        }
    }

    private void Detected()
    {
    
    }

    private void Undected()
    {
    
    }
}




