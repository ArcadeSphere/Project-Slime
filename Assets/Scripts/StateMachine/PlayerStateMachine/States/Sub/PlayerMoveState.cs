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


        player.SetVelocity(playerCore.MovementSpeed * xinput);

        if (xinput == 0)
        {
         
            stateMachine.PlayerChangeState(player.idleState);
        }
        else
        {

            player.PlayerShouldFlip(xinput);
        }
    }
    

    public override void PLayerPhysics()
    {
        base.PLayerPhysics();
    }
}
