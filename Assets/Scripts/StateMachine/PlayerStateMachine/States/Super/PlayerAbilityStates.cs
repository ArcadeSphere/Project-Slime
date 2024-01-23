using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilityStates : PlayerState
{
    protected bool isAbilityFinish;
    protected bool isGrounded;
    public PlayerAbilityStates(Player player, PlayerSateMachine stateMachine, PlayerCore playerCore, string animBoolName) : base(player, stateMachine, playerCore, animBoolName)
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
        isAbilityFinish = false;
    }

    public override void PLayerExitState()
    {
        base.PLayerExitState();
    }

    public override void PLayerLogic()
    {
        base.PLayerLogic();
        if (isAbilityFinish)
        {
            if (isGrounded && player.playerRb.velocity.y < 0.01f)
            {
                stateMachine.PlayerChangeState(player.idleState);
            }
            else
            {
                stateMachine.PlayerChangeState(player.airState);
            }
        }
    }

    public override void PLayerPhysics()
    {
        base.PLayerPhysics();
    }
}
