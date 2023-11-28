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
        
        [Header("Bobbing")]
        public float runningBobbingSpeed = 14f;
        public float runningBobbingAmount = 0.05f;

        private float defaultPosY = 0;
        private float defaultPosX = 0;
        float timer = 0;
        
        
        void Start()
        {
            camera = Camera.main;
            defaultPosY = camera.transform.localPosition.y;
            defaultPosX = camera.transform.localPosition.x;
        }

        // Update is called once per frame
        void Update()
        {
            HeadBobing();
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

