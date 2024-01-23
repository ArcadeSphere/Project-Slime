using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundState
{
    public PlayerMoveState(Player player, PlayerSateMachine stateMachine, PlayerCore playerCore, string animBoolName) : base(player, stateMachine, playerCore, animBoolName)
    {
    }

    public override void CheckForSomething()
    {
        base.CheckForSomething();
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
        player.PlayerShouldFlip(input);
        float horizontalVelocity = input * playerCore.MovementSpeed;
        player.setVelocity(horizontalVelocity);
        if (input == 0f)
        {
            stateMachine.PlayerChangeState(player.idleState);
        }
    }


    public override void PLayerPhysics()
    {
        base.PLayerPhysics();
    }
}
