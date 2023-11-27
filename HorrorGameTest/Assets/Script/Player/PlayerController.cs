using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        public Camera playerCamera;
        [Header("Player controls")]
        public float walkSpeed = 6f;
        [SerializeField]private float runSpeed = 12f;
        [SerializeField]private float jumpPower = 7f;
        [SerializeField]private float gravity = 10f;


        [SerializeField]private float lookSpeed = 2f;
        [SerializeField]private float lookXLimit = 45f;

        
        [Header("Player interaction")] 
        [SerializeField] private float raycastLenght = 10;

        public Vector3 moveDirection = Vector3.zero;
        float rotationX = 0;

        public bool canMove = true;

        public Vector3 mouseWorldPosition;
        public Vector2 mousePosition;
        
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

