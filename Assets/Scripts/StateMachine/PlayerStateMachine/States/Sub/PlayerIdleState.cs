using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerGroundState
{
    public PlayerIdleState(Player player, PlayerSateMachine stateMachine, PlayerCore playerCore, string animBoolName) : base(player, stateMachine, playerCore, animBoolName)
    {
    }

    public override void CheckForSomething()
    {
        base.CheckForSomething();
    }

    public override void PlayerEnterState()
    {
        base.PlayerEnterState();
        player.setVelocity(0f);
    }

    public override void PLayerExitState()
    {
        base.PLayerExitState();
    }

    public override void PLayerLogic()
    {
        base.PLayerLogic();
        if(input != 0)
        {
            Debug.Log("player move");
            stateMachine.PlayerChangeState(player.moveState);
        }
    }

    public override void PLayerPhysics()
    {
        base.PLayerPhysics();
    }
}
