using System.Collections;
using System.Collections.Generic;
using CameraBehavior;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;


namespace Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        public Camera playerCamera;
        [Header("Player controls")]
        [Header("Movement")]
        public bool canMove = true;
        [SerializeField]internal float walkSpeed = 6f;
        [SerializeField]private float runSpeed = 12f;
        [SerializeField]private float jumpPower = 7f;
        [SerializeField]private float gravity = 10f;
        
        [SerializeField]private float lookSpeed = 2f;
        [SerializeField]private float lookXLimit = 45f;

        [HideInInspector]public Vector3 moveDirection = Vector3.zero;
        float rotationX = 0;

        [Header("Crouch")]
        public bool isCrouching;
        [SerializeField] private float crouchWalkSpeed = 3f;
        private float baseWalkSpeed;
        

        [Header("Player interaction")] 
        [SerializeField] private GameObject light;
        private bool isLightEquipped; 
        [SerializeField] private float raycastLenght = 10;
        

        private Vector3 mouseWorldPosition;
        private Vector2 mousePosition;
        
        public CharacterController characterController;
        public static PlayerController Instance;

        private void Awake()
        {
            Instance = this;
        }
        
        void Start()
        {
            playerCamera = Camera.main;
            characterController = GetComponent<CharacterController>();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            isLightEquipped = true;
            isCrouching = false;
            baseWalkSpeed = walkSpeed;
        }
        
        void Update()
        {
            mousePosition = Input.mousePosition;
            Debug.DrawRay(Camera.main.transform.position,Camera.main.transform.forward * raycastLenght,Color.yellow);
            Movement();

            mouseWorldPosition = Vector3.zero;
            RaycastHit hit;
            Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
            Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
            if (Physics.Raycast(ray,out hit,999))
            {
                mouseWorldPosition = hit.point;
            }
        }

        
        private void OnInteract(InputValue value)
        {
            isLightEquipped = !isLightEquipped;
            light.SetActive(isLightEquipped);
        }
        private void OnCrouch(InputValue value)
        {
            isCrouching = !isCrouching;
            walkSpeed = isCrouching ? crouchWalkSpeed : baseWalkSpeed;
        }
        
        
        private void Movement()
        {
            #region Handles Movment
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 right = transform.TransformDirection(Vector3.right);

            // Press Left Shift to run
            bool isRunning = Input.GetKey(KeyCode.LeftShift);
            float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0;
            float curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0;
            float movementDirectionY = moveDirection.y;
            moveDirection = (forward.normalized * curSpeedX) + (right.normalized * curSpeedY) ;

            if (isRunning) isCrouching = false;
            
            CameraManager.instance.ChangeState(isCrouching
                ? CameraManager.CameraState.Crouch
                : CameraManager.CameraState.Normal);
            #endregion

            #region Handles Jumping
            HandlesJumping(movementDirectionY);
            #endregion

            #region Handles Rotation
            HandlesRotation();
            #endregion
        }

        private void HandlesJumping(float movementDirectionY)
        {
            if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
            {
                moveDirection.y = jumpPower;
            }
            else
            {
                moveDirection.y = movementDirectionY;
            }

            if (!characterController.isGrounded)
            {
                moveDirection.y -= gravity * Time.deltaTime;
            }
        }

        private void HandlesRotation()
        {
            characterController.Move(moveDirection * Time.deltaTime);

            if (canMove)
            {
                rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
                rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
                playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
                transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
            }
        }
    }
}

