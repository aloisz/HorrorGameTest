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
            Running,
            Crouch
        }
        public CameraState state;

        [Header("CrouchCamera")] 
        [SerializeField] private float timertoCrouch = .25f; 
        [SerializeField] private Vector3 crouchCameraPos;
        
        [Space]
        public float runningBobbingSpeed = 14f;
        public float runningBobbingAmount = 0.05f;

        private float defaultPosY = 0; // defaults camera position on y axis
        private float defaultPosX = 0; // defaults camera position on x axis

        [Space] 
        [Header("Camera Boobing")]
        [Header("Running")]
        public AnimationCurve BobbingCameraRunningCurve;
        [SerializeField] private float durationRunning = 1.0f; // Animation curve Duration
        [SerializeField] private float maxHeightXRunning = 3.0f; 
        [SerializeField] private float maxHeightYRunning = 3.0f;
        [SerializeField] private float maxHeightZRunning = 3.0f;
        
        [Space]
        [SerializeField] private float maxRotationXRunning = 20f;
        [SerializeField] private float maxRotationZRunning = 20f;
        [SerializeField] private float timeToReachMaxValue = 3; // Time to reach the max effect on camera
        
        [Header("Idle")]
        public AnimationCurve BobbingCameraIdleCurve;
        [SerializeField] private float durationIdle = 1.0f;
        [SerializeField] private float maxHeightXIdle = 3.0f;
        [SerializeField] private float maxHeightYIdle = 3.0f;
        [SerializeField] private float maxHeightZIdle = 3.0f;
        
        [Space]
        [SerializeField] private float maxRotationXIdle = 20f;
        [SerializeField] private float maxRotationZIdle = 20f;
        
        
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

            //Running 
            baseMaxRotationXRunning = maxRotationXRunning;
            baseMaxRotationZRunning = maxRotationZRunning;
            
            //Idle
            baseMaxRotationXIdle = maxRotationXIdle;
            baseMaxRotationZIdle = maxRotationZIdle;
        }
        
        void LateUpdate()
        {
            switch (state)
            {
                case CameraState.Normal:
                    break;
                case CameraState.Crouch:
                    Crouch();
                    break;
                case CameraState.Running:
                    break;
            }
            if(!PlayerController.Instance.isCrouching) HeadBobing();
        }

        // Change The camera State Behavior
        public void ChangeState(CameraState state)
        {
            this.state = state;
        }

        private void Crouch()
        {
            camera.transform.localPosition = 
                new Vector3(camera.transform.localPosition.x,Mathf.Lerp(defaultPosY, crouchCameraPos.y,timertoCrouch)
                    ,camera.transform.localPosition.z);
        }


        private bool doOnce = false;
        private void HeadBobing()
        {
            UpdateValueWhenSprinting();
            UpdateValueWhenIdle();
            if(PlayerController.Instance.isRunning ) //Player is running
            {
                ChangeState(CameraState.Running);
                if (!doOnce)
                {
                    doOnce = true;
                    StartCoroutine(CoroutineBobbingCameraRunningCurve(camera.gameObject,camera.transform.localPosition, 
                        new Vector3(defaultPosX, defaultPosY, 0)));
                }
            }
            else //Idle
            { 
                ChangeState(CameraState.Normal);
                if (!doOnceIdle && !doOnce )
                { 
                    //TODO : Wait for the previous anim curve to finish 
                    
                    doOnceIdle = true;
                    StartCoroutine(CoroutineBobbingCameraIdleCurve(camera.gameObject,camera.transform.localPosition, 
                        new Vector3(defaultPosX, defaultPosY, 0)));
                }
            }
        }

        private bool doOnceMaxRotationZ = false; // Check if the value need to be negative or positive
        private IEnumerator CoroutineBobbingCameraRunningCurve(GameObject obj, Vector3 start, Vector3 finish)
        {
            var timePast = 0f;
            float randomValueX = Random.Range(-maxHeightXRunning, maxHeightXRunning);
            
            doOnceMaxRotationZ = doOnce;
            yield return null;
            //maxRotationZRunning = doOnceMaxRotationZ ? -maxRotationZRunning : maxRotationZRunning; // one time positive one time negative

            while (timePast < durationRunning)
            {
                timePast += Time.deltaTime;
                var linearTime = timePast / durationRunning;
                var heightTime = BobbingCameraRunningCurve.Evaluate(linearTime);

                var heightX = Mathf.Lerp(0, randomValueX, heightTime); //clamped between the max height and 0
                var heightY = Mathf.Lerp(0f, maxHeightYRunning, heightTime); 
                var heightZ = Mathf.Lerp(0f, maxHeightZRunning, heightTime); 


                var rotZ = Mathf.Lerp(0, maxRotationZRunning, heightTime);
                var rotX = Mathf.Lerp(0, maxRotationXRunning, heightTime);

                // Camera Position
                obj.transform.localPosition = Vector3.Lerp(start, finish, linearTime) + new Vector3(heightX, heightY, heightZ);
                
                // Camera Rotation
                obj.transform.localEulerAngles = new Vector3(
                    rotX + PlayerController.Instance.rotationX,
                    0,
                    rotZ); 
                
                yield return null;
            }
            doOnce = false;
        }
        
        

        /// <summary>
        /// When Sprinting The value of the camera will increment by time
        /// </summary>
        private float incrementValueOverTime = 0; 
        private float baseMaxRotationXRunning; // Get the value of MaxRotationX to modify it 
        private float baseMaxRotationZRunning;// Get the value of MaxRotationY to modify it 
        private int check; // just to check if the balance of camera is on right or left side
        private void UpdateValueWhenSprinting()
        {
            if (PlayerController.Instance.isRunning )
            {
                if(incrementValueOverTime <= timeToReachMaxValue ) incrementValueOverTime += Time.deltaTime * 1;
                if (!doOnce)
                {
                    check++;
                }
            }
            else
            {
                if(incrementValueOverTime >= 0 ) incrementValueOverTime -= Time.deltaTime * 1;
            }
            maxRotationXRunning =  baseMaxRotationXRunning * incrementValueOverTime / timeToReachMaxValue;
            
            // One time positive value one time negative value 
            if(check % 2 == 0) maxRotationZRunning = baseMaxRotationZRunning * incrementValueOverTime / timeToReachMaxValue;
            else maxRotationZRunning = -baseMaxRotationZRunning * incrementValueOverTime / timeToReachMaxValue;
        }



        private bool doOnceIdle;
        private IEnumerator CoroutineBobbingCameraIdleCurve(GameObject obj, Vector3 start, Vector3 finish)
        {
            var timePast = 0f;
            float randomValueX = Random.Range(-maxHeightXIdle, maxHeightXIdle);
            
            yield return null;

            while (timePast < durationIdle)
            {
                timePast += Time.deltaTime;
                var linearTime = timePast / durationIdle;
                var heightTime = BobbingCameraIdleCurve.Evaluate(linearTime);

                var heightX = Mathf.Lerp(0, randomValueX, heightTime); //clamped between the max height and 0
                var heightY = Mathf.Lerp(0f, maxHeightYIdle, heightTime); 
                var heightZ = Mathf.Lerp(0f, maxHeightZIdle, heightTime); 


                var rotZ = Mathf.Lerp(0, maxRotationZIdle, heightTime);
                var rotX = Mathf.Lerp(0, maxRotationXIdle, heightTime);

                // Camera Position
                obj.transform.localPosition = Vector3.Lerp(start, finish, linearTime) + new Vector3(heightX, heightY, heightZ);
                
                // Camera Rotation
                obj.transform.localEulerAngles = new Vector3(
                    rotX + PlayerController.Instance.rotationX,
                    0,
                    rotZ); 
                
                yield return null;
            }
            doOnceIdle = false;
        }
        
        
        
        private float baseMaxRotationXIdle; // Get the value of MaxRotationX to modify it 
        private float baseMaxRotationZIdle;// Get the value of MaxRotationY to modify it 
        private void UpdateValueWhenIdle()
        {
            maxRotationXIdle =  baseMaxRotationXIdle * incrementValueOverTime / timeToReachMaxValue;
            maxRotationZIdle = baseMaxRotationZIdle * incrementValueOverTime / timeToReachMaxValue;
            
        }
    }
}

