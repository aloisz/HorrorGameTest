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
        public PlayerState currentState;
        [Header("Player controls")]
        [Header("Movement")]
        internal bool canMove = true;
        internal  bool isWalking;
        internal  bool isRunning;
        [Header("Movement")][SerializeField]internal float walkSpeed = 6f;
        [SerializeField]private float runSpeed = 12f;
        [SerializeField]private float jumpPower = 7f;
        [SerializeField]private float gravity = 10f;

        internal Vector2 lookPos;
        [SerializeField]private float lookSpeed = 2f;
        [SerializeField]private float lookXLimit = 45f;

        [HideInInspector]public Vector3 moveDirection = Vector3.zero;
        internal float rotationX = 0; 

        [Header("Crouch")]
        internal bool isCrouching;
        [Header("Crouch")][SerializeField] private float crouchWalkSpeed = 3f;
        private float baseWalkSpeed;
        
        internal CharacterController characterController;
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
            
            isCrouching = false;
            baseWalkSpeed = walkSpeed;
        }
        
        void Update()
        {
            Movement();
            HandlesRotation();
            SetLogicWhenChangingState();
        }
        
        public void ChangeState(PlayerState state)
        {
            this.currentState = state;
        }
        
        #region Movement 

        private void Movement()
        {
            if (!canMove) return;
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 right = transform.TransformDirection(Vector3.right);
            
            float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0;
            float curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0;
            float movementDirectionY = moveDirection.y;
            moveDirection = (forward.normalized * curSpeedX) + (right.normalized * curSpeedY);
            
            isWalking = moveDirection != Vector3.zero; // check if walking

            if (isRunning) // set logic when running
            {
                isCrouching = false;
                isWalking = false;
            }
            walkSpeed = isCrouching ? crouchWalkSpeed : baseWalkSpeed; // set walk speed when crouching

            HandlesJumping(movementDirectionY);
            characterController.Move(moveDirection * Time.deltaTime);
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

        private float posY;
        private float posX;
        private void HandlesRotation()
        {
            rotationX += -lookPos.y * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, lookPos.x * lookSpeed, 0);
        }

        #endregion
        
        
        private void SetLogicWhenChangingState()
        {
            if (characterController.isGrounded)
            {
                if (!isWalking)
                {
                    ChangeState(isRunning ? PlayerState.Running : PlayerState.Idle);
                }
                else
                {
                    ChangeState(PlayerState.Walking);
                }
            }
            else
            {
                ChangeState(PlayerState.Jumping);
            }
            
        }
    }
}


public enum PlayerState
{
    Idle,
    Walking,
    Running,
    Crouch,
    Jumping
}

