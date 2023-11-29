using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.Serialization;


namespace CameraBehavior
{
    public class CameraManager : MonoBehaviour
    {
        public Camera camera;
        
        public enum CameraState
        {
            Normal,
            Crouch
        }
        public CameraState state;

        [Header("CrouchCamera")] 
        [SerializeField] private float timertoCrouch = .25f; 
        [SerializeField] private Vector3 crouchCameraPos;
        
        [Header("Bobbing")]
        public float runningBobbingSpeed = 14f;
        public float runningBobbingAmount = 0.05f;

        private float defaultPosY = 0;
        private float defaultPosX = 0;
        float timer = 0;

        public static CameraManager instance;

        private void Awake()
        {
            instance = this;
        }
        void Start()
        {
            camera = Camera.main;
            defaultPosY = camera.transform.localPosition.y;
            defaultPosX = camera.transform.localPosition.x;
        }
        
        void LateUpdate()
        {
            switch (state)
            {
                case CameraState.Normal:
                    HeadBobing();
                    break;
                case CameraState.Crouch:
                    Crouch();
                    break;
            }
        }

        public void ChangeState(CameraState state)
        {
            this.state = state;
        }

        private void Crouch()
        {
            camera.transform.localPosition = 
                new Vector3(camera.transform.localPosition.x,Mathf.Lerp(camera.transform.localPosition.y, crouchCameraPos.y,timertoCrouch)
                    ,camera.transform.localPosition.z);
        }
        
        private void HeadBobing()
        {
            if(Mathf.Abs(PlayerController.Instance.moveDirection.x) > PlayerController.Instance.walkSpeed +2 || Mathf.Abs(PlayerController.Instance.moveDirection.z) > PlayerController.Instance.walkSpeed+2 
               && PlayerController.Instance.characterController.isGrounded)
            {
                //Player is moving
                timer += Time.deltaTime * runningBobbingSpeed;
                camera.transform.localPosition = new Vector3(defaultPosX + Mathf.Sin(timer) * runningBobbingAmount, defaultPosY + Mathf.Sin(timer) * runningBobbingAmount, camera.transform.localPosition.z);
            }
            else 
            {
                //Idle
                timer = 0;
                camera.transform.localPosition = new Vector3(Mathf.Lerp(camera.transform.localPosition.x, defaultPosX, Time.deltaTime * runningBobbingSpeed), Mathf.Lerp(camera.transform.localPosition.y, defaultPosY, Time.deltaTime * runningBobbingSpeed), camera.transform.localPosition.z);
            }
        }
    }
}

