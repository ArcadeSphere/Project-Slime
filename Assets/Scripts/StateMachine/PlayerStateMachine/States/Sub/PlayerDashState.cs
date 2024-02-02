using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerAbilityStates
{
    public bool canDash { get; private set; }
    private float lastDashTime;
    private float originalGravityScale;

    public PlayerDashState(Player player, PlayerSateMachine stateMachine, PlayerCore playerCore, string animBoolName) : base(player, stateMachine, playerCore, animBoolName)
    {
    }

    public bool CheckIfCanDash()
    {
        return canDash && Time.time >= lastDashTime + playerCore.DashCooldown;
    }
    public void RestDash()
    {
        canDash = true;
    }

    public override void PlayerEnterState()
    {
        base.PlayerEnterState();
        canDash = false;
        originalGravityScale = player.playerRb.gravityScale;
        player.playerRb.gravityScale = 0f;
        player.playerinput.UseDashInput();

    }

    public override void PLayerExitState()
    {
        base.PLayerExitState();
        player.playerRb.gravityScale = originalGravityScale;

    }

    public override void PLayerLogic()
    {
        base.PLayerLogic();

        if (lastDashTime > 0)
        {
            float dashVelocity = playerCore.DashSpeed * Mathf.Sign(player.transform.localScale.x);
            Vector2 dashVelocityVector = new Vector2(dashVelocity, 0f);
            player.SetDashVelocity(dashVelocityVector);
            lastDashTime -= Time.deltaTime;
        }

        else if (lastDashTime <= 0 && !isAbilityFinish)
        {
            lastDashTime = playerCore.DashTime;
            isAbilityFinish = true;
            if (isGrounded)
            {
                stateMachine.PlayerChangeState(player.idleState);
            }
            else
            {
                stateMachine.PlayerChangeState(player.airState);
            }
        }
    }
}
