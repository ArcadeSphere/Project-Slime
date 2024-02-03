using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLandState : PlayerGroundState
{
    public PlayerLandState(Player player, PlayerSateMachine stateMachine, PlayerCore playerCore, string animBoolName) : base(player, stateMachine, playerCore, animBoolName)
    {
    }

    public override void PLayerLogic()
    {
        base.PLayerLogic();
        if (!isExitState)
        {
            if (xinput != 0)
            {
                stateMachine.PlayerChangeState(player.moveState);
            }
            else if (isAnimFinish)
            {
                stateMachine.PlayerChangeState(player.idleState);
            }
        }
    }
      
}
