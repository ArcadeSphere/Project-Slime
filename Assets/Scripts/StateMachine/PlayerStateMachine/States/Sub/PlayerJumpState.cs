using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerAbilityStates
{
    private int amountOfJumpsLeft;
    public PlayerJumpState(Player player, PlayerSateMachine stateMachine, PlayerCore playerCore, string animBoolName) : base(player, stateMachine, playerCore, animBoolName)
    {
        amountOfJumpsLeft = playerCore.AmountOfJumps;
    }

    public override void PlayerEnterState()
    {
        base.PlayerEnterState();
        player.playerinput.UseJumpInput();
        player.SetJumpVelocity(playerCore.JumpVelocity);
        isAbilityFinish = true;
        amountOfJumpsLeft--;
        player.airState.SetisJumping();
    }

    public bool CanJump()
    {
        if(amountOfJumpsLeft > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void ResetAmountOfJumps()
    {
        amountOfJumpsLeft = playerCore.AmountOfJumps;
    }
    public void DecreaseAmountofJumps()
    {
        amountOfJumpsLeft--;
    }
}
