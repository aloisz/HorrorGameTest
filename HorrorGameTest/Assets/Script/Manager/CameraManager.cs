using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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
            Idle,
            Walking,
            Running,
            Crouch
        }
        public CameraState currentState;

        [Header("CrouchCamera")] 
        [SerializeField] private float timertoCrouch = .25f; 
        [SerializeField] private Transform crouchCameraPos;
        
        [Space]
        private float defaultPosY = 0; // defaults camera position on y axis
        private float defaultPosX = 0; // defaults camera position on x axis

        
        [Space] 
        [Header("Camera Boobing")]
        
        [SerializeField] private float timeToReachMaxValue = 3; // Time to reach the max effect on camera
        [Header("Running")]
        private bool doOnceRunning = false;
        public AnimationCurve BobbingCameraRunningCurve;
        [SerializeField] private float durationRunning = 1.0f; // Animation curve Duration
        [SerializeField] private float maxHeightXRunning = 3.0f; 
        [SerializeField] private float maxHeightYRunning = 3.0f;
        [SerializeField] private float maxHeightZRunning = 3.0f;
        
        [Space]
        [SerializeField] private float maxRotationXRunning = 20f;
        [SerializeField] private float maxRotationZRunning = 20f;
        
        [Header("Idle")]
        private bool doOnceIdle;
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
            UpdateValueWhenSprinting();
            UpdateValueWhenIdle();

            switch (currentState)
            {
                case CameraState.Idle:
                    BoobingIdle();
                    break;
                case CameraState.Crouch:
                    Crouch();
                    break;
                case CameraState.Running:
                    BoobingRunning();
                    break;
                case CameraState.Walking:
                    
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        // Change The State Behavior
        public void ChangeState(CameraState state)
        {
            this.currentState = state;
        }

        
        
        /// <summary>
        /// Idle Management here
        /// </summary>
        #region Idle

        private void BoobingIdle()
        {
            if (doOnceIdle || doOnceRunning) return;
            doOnceIdle = true;
            StartCoroutine(CoroutineBobbingCameraIdleCurve(camera.gameObject,camera.transform.localPosition, 
                new Vector3(defaultPosX, defaultPosY, 0)));
        }
        
        
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

        #endregion
        
        /// <summary>
        /// Running Management here
        /// </summary>
        #region Running
        
        
        private void BoobingRunning()
        {
            if (doOnceRunning || doOnceIdle) return;
            doOnceRunning = true;
            StartCoroutine(CoroutineBobbingCameraRunningCurve(camera.gameObject,camera.transform.localPosition, 
                new Vector3(defaultPosX, defaultPosY, 0)));
        }
        
        /// <summary>
        /// Read the animation curve and add effect to the camera
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="start"></param>
        /// <param name="finish"></param>
        /// <returns></returns>
        private IEnumerator CoroutineBobbingCameraRunningCurve(GameObject obj, Vector3 start, Vector3 finish)
        {
            var timePast = 0f;
            float randomValueX = Random.Range(-maxHeightXRunning, maxHeightXRunning);
            
            yield return null;

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
            doOnceRunning = false;
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
                if (!doOnceRunning)
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

        #endregion


        #region Crouch

        private void Crouch()
        {
            StopAllCoroutines();
            camera.transform.DOMove(crouchCameraPos.position, timertoCrouch);
        } 

        #endregion
        
        
    }
}

