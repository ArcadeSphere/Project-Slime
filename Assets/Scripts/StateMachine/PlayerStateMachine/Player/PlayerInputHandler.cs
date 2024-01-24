using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
public class PlayerInputHandler : MonoBehaviour
{
   public InputManager inputManager;

    #region Horizonatal
    public float horizontalInput { get; private set; }

    public int normalizeInputX { get; private set; }
    public int normalizeInputY { get; private set; }

    #endregion

    #region Jump
    public bool jumpInput { get; private set; }
    public bool jumpInputStop { get; private set; }

    private float jumpInputStartTime;
    #endregion

    #region Dash
    public bool dashInput { get; private set; }

    private float dashInputStartTime;

    #endregion


    [SerializeField] private float inputHoldtime = 0.2f;
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
    }

    public void OnJumpInput()
    {
        if (inputManager.GetJumpInput())
        {

            jumpInput = true;
            jumpInputStop = false;
            jumpInputStartTime = Time.time;
        }
        if (inputManager.GetJumpInputUp())
        {
            jumpInputStop = true;
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

    public void OnDashInput()
    {
        if (inputManager.GetDashInputDown())
        {
            dashInput = true;
            dashInputStartTime = Time.time;
        }
    }

    public void UseDashInput()
    {
        dashInput = false;
    }

}


