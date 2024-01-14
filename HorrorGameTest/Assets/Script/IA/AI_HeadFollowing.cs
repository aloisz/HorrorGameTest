using System;
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


        private void Start()
        {
            whatToLookAt = PlayerController.Instance.transform;
        }

        void LateUpdate()
        {
            LookAt();
        }

        internal void WhatToLookAt(Transform whatToLookAt)
        {
            this.whatToLookAt = whatToLookAt;
        }

        private void LookAt()
        {
            if(whatToLookAt != null) aiHead.DOLookAt(whatToLookAt.transform.position, timeToChangeLookAt);
        }
        
        
    }
}

