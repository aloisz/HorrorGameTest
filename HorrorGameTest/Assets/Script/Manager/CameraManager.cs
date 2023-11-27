using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;


namespace CameraBehavior
{
    public class CameraManager : MonoBehaviour
    {
        public Camera camera;
        
        [Header("Bobbing")]
        public float walkingBobbingSpeed = 14f;
        public float bobbingAmount = 0.05f;

        [SerializeField] private float defaultPosY = 0;
        float timer = 0;
        
        
        void Start()
        {
            camera = Camera.main;
            defaultPosY = camera.transform.localPosition.y;
        }

        // Update is called once per frame
        void Update()
        {
            HeadBobing();
        }
        
        private void HeadBobing()
        {
            if(Mathf.Abs(PlayerController.Instance.moveDirection.x) > PlayerController.Instance.walkSpeed || Mathf.Abs(PlayerController.Instance.moveDirection.z) > PlayerController.Instance.walkSpeed 
               && PlayerController.Instance.characterController.isGrounded)
            {
                //Player is moving
                timer += Time.deltaTime * walkingBobbingSpeed;
                camera.transform.localPosition = new Vector3(camera.transform.localPosition.x, defaultPosY + Mathf.Sin(timer) * bobbingAmount, camera.transform.localPosition.z);
            }
            else
            {
                //Idle
                timer = 0;
                camera.transform.localPosition = new Vector3(camera.transform.localPosition.x, Mathf.Lerp(camera.transform.localPosition.y, defaultPosY, Time.deltaTime * walkingBobbingSpeed), camera.transform.localPosition.z);
            }
        }
    }
}

