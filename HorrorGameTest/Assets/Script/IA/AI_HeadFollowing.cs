using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Player;
using UnityEngine;



namespace AI
{
    public class AI_HeadFollowing : MonoBehaviour
    {

        [SerializeField] private Transform aiHead;
        [SerializeField] private Transform whatToLookAt; // what the IA have to look at
        [SerializeField] private float timeToChangeLookAt = .325f;

        
        void LateUpdate()
        {
            aiHead.DOLookAt(whatToLookAt.transform.position, timeToChangeLookAt );
        }

        internal void WhatToLookAt(Transform whatToLookAt)
        {
            this.whatToLookAt = whatToLookAt;
        }
        
        
    }
}

