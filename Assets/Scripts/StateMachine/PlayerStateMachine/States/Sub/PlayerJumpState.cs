using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerAbilityStates
{
    public PlayerJumpState(Player player, PlayerSateMachine stateMachine, PlayerCore playerCore, string animBoolName) : base(player, stateMachine, playerCore, animBoolName)
    {
        
    }

    public override void PlayerEnterState()
    {
        base.PlayerEnterState();
        player.SetJumpVelocity(playerCore.JumpVelocity);
        isAbilityFinish = true;
    }
}
