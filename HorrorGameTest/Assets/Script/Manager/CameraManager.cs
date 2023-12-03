using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;


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

        [Space] 
        public AnimationCurve BobbingCameraRunningCurve;
        [SerializeField] private float duration = 1.0f;
        [SerializeField] private float maxHeightX = 3.0f;
        [SerializeField] private float maxHeightY = 3.0f;
        
        [SerializeField] private float maxRotationX = 20f;
        [SerializeField] private float maxRotationZ = 20f;
        
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


        private bool doOnce = false;
        private void HeadBobing()
        {
            if(Mathf.Abs(PlayerController.Instance.moveDirection.x) > PlayerController.Instance.walkSpeed +2 || Mathf.Abs(PlayerController.Instance.moveDirection.z) > PlayerController.Instance.walkSpeed+2 &&
                PlayerController.Instance.characterController.isGrounded)
            {
                //Player is moving
                timer += Time.deltaTime * runningBobbingSpeed;
                if (!doOnce)
                {
                    doOnce = true;
                    StartCoroutine(CoroutineBobbingCameraRunningCurve(camera.gameObject,camera.transform.localPosition, camera.transform.localPosition));
                }
                //camera.transform.localPosition = new Vector3(defaultPosX + Mathf.Sin(timer) * runningBobbingAmount, defaultPosY + Mathf.Sin(timer) * runningBobbingAmount, camera.transform.localPosition.z);
            }
            else 
            {
                //Idle
                timer = 0;
                camera.transform.localPosition = new Vector3(Mathf.Lerp(camera.transform.localPosition.x, defaultPosX, Time.deltaTime * runningBobbingSpeed), Mathf.Lerp(camera.transform.localPosition.y, defaultPosY, Time.deltaTime * runningBobbingSpeed), camera.transform.localPosition.z);
            }
        }

        private bool doOnceMaxRotationZ = false; // Check if the value need to be negative or positive
        private IEnumerator CoroutineBobbingCameraRunningCurve(GameObject obj, Vector3 start, Vector3 finish)
        {
            var timePast = 0f;
            float randomValueX = Random.Range(-maxHeightX, maxHeightX);
            
            doOnceMaxRotationZ = doOnce;
            yield return null;
            maxRotationZ = doOnceMaxRotationZ ? -maxRotationZ : maxRotationZ;
            //maxRotationX = doOnceMaxRotationZ ? -maxRotationX : maxRotationX;
            
            while (timePast < duration)
            {
                timePast += Time.deltaTime;
                var linearTime = timePast / duration;
                var heightTime = BobbingCameraRunningCurve.Evaluate(linearTime); 

                var heightX = Mathf.Lerp(0, randomValueX, heightTime); //clamped between the max height and 0
                var heightY = Mathf.Lerp(0f, maxHeightY, heightTime); //clamped between the max height and 0

                var rotZ = Mathf.Lerp(0, maxRotationZ, heightTime);
                var rotX = Mathf.Lerp(0, maxRotationX, heightTime);

                obj.transform.localPosition = Vector3.Lerp(start, finish, linearTime) + new Vector3(heightX, heightY, 0f);
                obj.transform.localEulerAngles = new Vector3(
                    rotX + PlayerController.Instance.rotationX,
                    0,
                    rotZ);
                
                yield return null;
            }
            doOnce = false;
        }
    }
}

