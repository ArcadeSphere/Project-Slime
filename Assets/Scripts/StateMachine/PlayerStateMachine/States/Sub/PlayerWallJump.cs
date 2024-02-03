using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallJump : PlayerAbilityStates
{
    public PlayerWallJump(Player player, PlayerSateMachine stateMachine, PlayerCore playerCore, string animBoolName) : base(player, stateMachine, playerCore, animBoolName)
    {
    }

    public override void PlayerEnterState()
    {
        base.PlayerEnterState();
        player.jumpState.ResetAmountOfJumps();
        int wallJumpDirection = -(int)(player.transform.localScale.x);
        player.WallJumpVelocity(playerCore.WallJumpPower, playerCore.WallJumpAnlge);
        player.PlayerShouldFlip(wallJumpDirection);
        Debug.Log($"Wall Jump Direction: {wallJumpDirection}, Angle: {playerCore.WallJumpAnlge}");
        player.jumpState.DecreaseAmountofJumps();
       
    }

    public override void PLayerLogic()
    {
        base.PLayerLogic();
        if(Time.time >= startTime + playerCore.WallJumpTimer)
        {
            isAbilityFinish = true;
        }

    }
}
