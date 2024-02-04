using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirState : PlayerState
{
    public bool isGrounded;
    private bool isWalled;
    private bool isBackWalled;
    private int input;
    private bool jumpInput;
    private bool coyoteTime;
    private bool isJumping;
    private bool jumpInputStop;
    private bool dashInput;
    public PlayerAirState(Player player, PlayerSateMachine stateMachine, PlayerCore playerCore, string animBoolName) : base(player, stateMachine, playerCore, animBoolName)
    {
    }

    public override void CheckForSomething()
    {
        base.CheckForSomething();
        isGrounded = player.CheckForGround();
        isWalled = player.CheckForWalls();
        isBackWalled = player.CheckForWallBack();
    }

    public override void PlayerEnterState()
    {
        base.PlayerEnterState();

    }

    public override void PLayerExitState()
    {
        base.PLayerExitState();
    }

    public override void PLayerLogic()
    {
        base.PLayerLogic();
        CheckForCoyoteTime();
        input = player.playerinput.normalizeInputX;
        jumpInput = player.playerinput.jumpInput;
        jumpInputStop = player.playerinput.jumpInputStop;
        dashInput = player.playerinput.dashInput;
        CheckForJumpMultiplyer();

        if (isGrounded && player.playerRb.velocity.y < 0.01f)
        {
        
            stateMachine.PlayerChangeState(player.landState);
        }
        else if(jumpInput && (isWalled || isBackWalled))
        {
            stateMachine.PlayerChangeState(player.wallJumpState);
        }
        else if(jumpInput && player.jumpState.CanJump())
        {
      
            stateMachine.PlayerChangeState(player.jumpState);
        }
        else if (isWalled && Mathf.Sign(input) == Mathf.Sign(player.transform.localScale.x) && player.playerRb.velocity.y <= 0) 
        {
             player.PlayerShouldFlip(input);
            stateMachine.PlayerChangeState(player.wallSlideState);
        }
        else
        {   
            player.SetVelocity(playerCore.MovementSpeed * input);
            player.PlayerShouldFlip(input);
        }

        if(dashInput && player.dashState.CheckIfCanDash() && ((input > 0 && player.transform.localScale.x > 0) || (input < 0 && player.transform.localScale.x < 0)))
        {
            stateMachine.PlayerChangeState(player.dashState);
        }

    }
    public override void PLayerPhysics()
    {
        base.PLayerPhysics();
    }
    public void CheckForJumpMultiplyer()
    {
        if (isJumping)
        {
            if (jumpInputStop)
            {
                player.SetJumpVelocity(player.playerRb.velocity.y * playerCore.jumpHeightMultiplier);
                isJumping = false;
            }
            else if (player.playerRb.velocity.y <= 0f)
            {
                isJumping = false;
            }
        }
    }
    private void CheckForCoyoteTime()
    {
        if (coyoteTime && Time.time > startTime + playerCore.coyoteTime)
        {
            coyoteTime = false;
            player.jumpState.DecreaseAmountofJumps();
        }
    }

    public void StartCoyoteTimer()
    {
        coyoteTime = true;
    }
    public void SetisJumping()
    {
        isJumping = true;
    }
}
