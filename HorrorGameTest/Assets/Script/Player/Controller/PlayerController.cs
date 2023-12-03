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
        [SerializeField]internal  bool isWalking;
        [SerializeField]internal  bool isRunning;
        [SerializeField]internal float walkSpeed = 6f;
        [SerializeField]private float runSpeed = 12f;
        [SerializeField]private float jumpPower = 7f;
        [SerializeField]private float gravity = 10f;

        private Vector2 lookPos;
        [SerializeField]private float lookSpeed = 2f;
        [SerializeField]private float lookXLimit = 45f;

        [HideInInspector]public Vector3 moveDirection = Vector3.zero;
        internal float rotationX = 0; 

        [Header("Crouch")]
        public bool isCrouching;
        [SerializeField] private float crouchWalkSpeed = 3f;
        private float baseWalkSpeed;


        [Header("Player interaction")] 
        [SerializeField] bool isInteracting;
        [SerializeField] private Transform hand;
        [SerializeField] private GameObject light;
        internal bool isLightEquipped; 
        [SerializeField] private float raycastLenght = 10;

        [Space] 
        [SerializeField] internal int numberOfBatterie;

        private Vector3 mouseWorldPosition;
        private Vector2 mousePosition;
        
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
            isLightEquipped = true;
            isCrouching = false;
            baseWalkSpeed = walkSpeed;
        }
        
        void Update()
        {
            mousePosition = Input.mousePosition;
            Movement();
            mouseWorldPosition = Vector3.zero;
            
            Debug.DrawRay(hand.position,hand.forward * raycastLenght,Color.yellow);

            light.SetActive(isLightEquipped);
        }

        
        public void OnInteract(InputAction.CallbackContext ctx)
        {
            if (ctx.started)
            {
                isInteracting = true;
                RaycastHit hit;
                if (Physics.Raycast(hand.position, hand.forward,out hit,raycastLenght))
                {
                    if (hit.transform.GetComponent<InteractiveObj>() != null)
                    {
                        hit.transform.GetComponent<InteractiveObj>().Interact(this);
                    }
                }
            }
            if (ctx.canceled)isInteracting = false;
        }
        
        public void OnUse(InputAction.CallbackContext ctx)
        {
            isLightEquipped = !isLightEquipped;
        }
        public void OnCrouch(InputAction.CallbackContext ctx)
        {
            isCrouching = !isCrouching;
        }
        
        public void OnRun(InputAction.CallbackContext ctx)
        {
            if (ctx.started)
            {
                isRunning = true;
                isCrouching = true;
            }
            
            if (ctx.canceled)
                isRunning = false;
        }

        public void OnLook(InputAction.CallbackContext ctx)
        {
            lookPos = ctx.ReadValue<Vector2>();
        }
        
        #region Mouvement 

        private void Movement()
        {
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 right = transform.TransformDirection(Vector3.right);
            
            //bool isRunning = Input.GetKey(KeyCode.LeftShift);
            float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0;
            float curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0;
            float movementDirectionY = moveDirection.y;
            moveDirection = (forward.normalized * curSpeedX) + (right.normalized * curSpeedY);
            
            isWalking = moveDirection != Vector3.zero; // check if walking

            if (isRunning) isCrouching = false;
            walkSpeed = isCrouching ? crouchWalkSpeed : baseWalkSpeed;
            
            CameraManager.instance.ChangeState(isCrouching
                ? CameraManager.CameraState.Crouch
                : CameraManager.CameraState.Normal);
            
            HandlesJumping(movementDirectionY);
            HandlesRotation();
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
            characterController.Move(moveDirection * Time.deltaTime);

            if (canMove)
            {
                rotationX += -lookPos.y * lookSpeed;
                rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
                playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
                transform.rotation *= Quaternion.Euler(0, lookPos.x * lookSpeed, 0);
                
                /*posY += lookPos.x * lookSpeed;
                posX -= lookPos.y * lookSpeed;

                Vector3 targetRotation = new Vector3(posX, posY);
                transform.eulerAngles = targetRotation;*/
            }
        }

        #endregion

        public void AddBatterie(int numberofBatterie)
        {
            numberOfBatterie += numberofBatterie;
        }

        public void RemoveBatterie(int numberofBatterie)
        {
            numberOfBatterie -= numberofBatterie;
        }

    }
}

