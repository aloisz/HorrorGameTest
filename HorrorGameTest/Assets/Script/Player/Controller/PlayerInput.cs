using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    
    public static PlayerInput Instance;
    private void Awake()
    {
        Instance = this;
    }
    
    
    
    // Player Controller --------------------
    
    public void OnCrouch(InputAction.CallbackContext ctx)
    {
        PlayerController.Instance.isCrouching = !PlayerController.Instance.isCrouching;
    }
        
    public void OnRun(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            PlayerController.Instance.isRunning = true;
            PlayerController.Instance.isCrouching = true;
        }
            
        if (ctx.canceled) PlayerController.Instance.isRunning = false;
    }

    public void OnLook(InputAction.CallbackContext ctx)
    {
        PlayerController.Instance.lookPos = ctx.ReadValue<Vector2>();
    }
    
    
    // Player Interaction --------------------
    
    public void OnInteract(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            PlayerInteraction.Instance.isInteracting = true;
            RaycastHit hit;
            if (Physics.Raycast(PlayerInteraction.Instance.hand.position, PlayerInteraction.Instance.hand.forward,out hit,PlayerInteraction.Instance.raycastLenght))
            {
                if (hit.transform.GetComponent<InteractiveObj>() != null)
                {
                    hit.transform.GetComponent<InteractiveObj>().Interact(PlayerController.Instance);
                }
            }
        }
        if (ctx.canceled)PlayerInteraction.Instance.isInteracting = false;
    }
        
    public void OnUse(InputAction.CallbackContext ctx)
    {
        PlayerInteraction.Instance.isLightEquipped = !PlayerInteraction.Instance.isLightEquipped;
    }
    
}
