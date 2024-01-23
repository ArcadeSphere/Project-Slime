using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirState : PlayerState
{
    public bool isGrounded;
    private int input;
    public PlayerAirState(Player player, PlayerSateMachine stateMachine, PlayerCore playerCore, string animBoolName) : base(player, stateMachine, playerCore, animBoolName)
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
    }

    public override void PLayerExitState()
    {
        base.PLayerExitState();
    }

    public override void PLayerLogic()
    {
        base.PLayerLogic();
        input = player.playerinput.normalizeInputX;

        if (isGrounded && player.playerRb.velocity.y < 0.01f)
        {
            Debug.Log("Landed");
            stateMachine.PlayerChangeState(player.landState);
        }
        else
        { 
           
            player.SetVelocity(playerCore.MovementSpeed * input);
            player.PlayerShouldFlip(input);

        }
    }
    public override void PLayerPhysics()
    {
        base.PLayerPhysics();
    }
}
