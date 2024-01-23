using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
   public InputManager inputManager;
   
    public float horizontalInput { get; private set; }

    public int normalizeInputX { get; private set; }
    public int normalizeInputY { get; private set; }

    public bool jumpInput { get; private set; }

    [SerializeField] private float inputHoldtime = 0.2f;

    private float jumpInputStartTime;

    private void Update()
    {
        CheckForHoldJumpTime();
    }
    public void OnMoveInput()
    {
        if (inputManager != null)
        {
            horizontalInput = inputManager.GetHorizontalInput();
            normalizeInputX = (int)(horizontalInput * Vector2.right).normalized.x;
            normalizeInputX = (int)(horizontalInput * Vector2.up).normalized.y;
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
            jumpInputStartTime = Time.time;
        }
    }
    public void UseJumpInput()
    {
        jumpInput = false;
    }
    private void CheckForHoldJumpTime()
    {
        if(Time.time >= jumpInputStartTime + inputHoldtime)
        {
            jumpInput = false;
        }
    }

}


