using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
   public InputManager inputManager;
   
    public float horizontalInput { get; private set; }
    public bool jumpInput { get; private set; }

    public void OnMoveInput()
    {
        if (inputManager != null)
        {
            horizontalInput = inputManager.GetHorizontalInput();
        }
        else
        {
            Debug.LogError("InputManager is not assigned to PlayerInputHandler.");
        }
    }

    public void OnJumpInput()
    {
        if (inputManager.GetJumpInput())
        {

            jumpInput = true;
        }
    }
    public void UseJumpInput()
    {
        jumpInput = false;
    }

}


