using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PLayerWallStates : PlayerState
{
    protected bool isGrounded;
    protected bool isWalled;
    protected bool jumpInput;
    protected int xInput;
    public PLayerWallStates(Player player, PlayerSateMachine stateMachine, PlayerCore playerCore, string animBoolName) : base(player, stateMachine, playerCore, animBoolName)
    {
    }

    public override void AnimationFinish()
    {
        base.AnimationFinish();
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
    }

    public override void CheckForSomething()
    {
        base.CheckForSomething();
        isGrounded = player.CheckForGround();
        isWalled = player.CheckForWalls();
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
        xInput = player.playerinput.normalizeInputX;
        jumpInput = player.playerinput.jumpInput;
        if (jumpInput)
        {
            stateMachine.PlayerChangeState(player.wallJumpState);
        }
        else if (isGrounded)
        {
            stateMachine.PlayerChangeState(player.idleState);
        }
        else if (!isWalled || (xInput != 0 && Mathf.Sign(xInput) != Mathf.Sign(player.transform.localScale.x)))
        {
            stateMachine.PlayerChangeState(player.airState);
        }
    }

    public override void PLayerPhysics()
    {
        base.PLayerPhysics();
    }
}
