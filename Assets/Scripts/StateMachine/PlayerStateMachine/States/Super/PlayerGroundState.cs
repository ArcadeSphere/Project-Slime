using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundState : PlayerState
{
    protected int xinput;
    private bool jumpInput;
    private bool isGrounded;
    private bool dashInput;
    public PlayerGroundState(Player player, PlayerSateMachine stateMachine, PlayerCore playerCore, string animBoolName) : base(player, stateMachine, playerCore, animBoolName)
    {
    }

    public override void CheckForSomething()
    {
        base.CheckForSomething();
        isGrounded = player.CheckForGround();
    }

    public override void PlayerEnterState()
    {
        base.PlayerEnterState();
        player.jumpState.ResetAmountOfJumps();
        player.dashState.RestDash();
    }

    public override void PLayerExitState()
    {
        base.PLayerExitState();
    }

    public override void PLayerLogic()
    {
        base.PLayerLogic();
        xinput = player.playerinput.normalizeInputX;
        jumpInput = player.playerinput.jumpInput;
        dashInput = player.playerinput.dashInput;

        if (jumpInput && player.jumpState.CanJump())
        {

         
            stateMachine.PlayerChangeState(player.jumpState);
        }
        else if (!isGrounded)
        {
            player.airState.StartCoyoteTimer();
            stateMachine.PlayerChangeState(player.airState);
        }

        if (dashInput && player.dashState.CheckIfCanDash())
        {
            stateMachine.PlayerChangeState(player.dashState);
        }
    }


    public override void PLayerPhysics()
    {
        base.PLayerPhysics();
    }
}
