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
        player.currentVelocity = playerCore.MovementSpeed * input;
    }

    public override void PLayerExitState()
    {
        base.PLayerExitState();
    }

    public override void PLayerLogic()
    {
        base.PLayerLogic();
        float targetVelocity = input * playerCore.AccelerationSpeed;
        player.currentVelocity = Mathf.MoveTowards(player.currentVelocity, targetVelocity, Mathf.Abs(playerCore.AccelerationSpeed) * Time.deltaTime);
        player.SetVelocity(player.currentVelocity);

        if (input == 0f)
        {
         
            stateMachine.PlayerChangeState(player.idleState);
        }
        else
        {
          
            player.Playerflip();
        }
    }
    

    public override void PLayerPhysics()
    {
        base.PLayerPhysics();
    }
}
