using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSlideState : PLayerWallStates
{
    public PlayerWallSlideState(Player player, PlayerSateMachine stateMachine, PlayerCore playerCore, string animBoolName) : base(player, stateMachine, playerCore, animBoolName)
    {
    }

    public override void PLayerLogic()
    {
        base.PLayerLogic();
        if (!isExitState)
        {
            player.SetJumpVelocity(-playerCore.wallSlideForce);
        }
     
    }
}
