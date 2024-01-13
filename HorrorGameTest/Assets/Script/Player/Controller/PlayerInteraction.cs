using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;


namespace Player
{
    public class PlayerInteraction : MonoBehaviour
    {
        [Header("Player interaction")] 
        [SerializeField] bool isInteracting;
        [SerializeField] private Transform hand;
        [SerializeField] private GameObject light;
        internal bool isLightEquipped; 
        [SerializeField] private float raycastLenght = 10;

        [Space] 
        [SerializeField] internal int numberOfBatterie;
    
        public static PlayerInteraction Instance;

        private void Awake()
        {
            Instance = this;
        }
        void Start()
        {
            isLightEquipped = true;
        }
        
        void Update()
        {
            Debug.DrawRay(hand.position,hand.forward * raycastLenght,Color.yellow);
            light.SetActive(isLightEquipped);

            LightIntensityManagement();
        }


        private void LightIntensityManagement()
        {
            RaycastHit hit;
            
            if (Physics.Raycast(hand.position, hand.forward,out hit,raycastLenght))
            {
                if (!(hit.distance < 2))
                {
                    if (hit.distance < 4)
                    {
                        Debug.DrawRay(hand.position, hand.forward * raycastLenght, Color.green);
                        light.GetComponent<Light>().intensity = 30;
                    }
                }
                else
                {
                    Debug.DrawRay(hand.position, hand.forward * raycastLenght, Color.red);
                    light.GetComponent<Light>().intensity = 10;
                }
            }
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
                        hit.transform.GetComponent<InteractiveObj>().Interact(PlayerController.Instance);
                    }
                }
            }
            if (ctx.canceled)isInteracting = false;
        }
        
        public void OnUse(InputAction.CallbackContext ctx)
        {
            isLightEquipped = !isLightEquipped;
        }
        
        
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

