using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerAbilityStates
{
    public bool canDash { get; private set; }
    private float lastDashTime;

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
    }

    public override void PLayerExitState()
    {
        base.PLayerExitState();
    }

    public override void PLayerLogic()
    {
        base.PLayerLogic();
    }
}
