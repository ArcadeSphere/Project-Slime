using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundState : PlayerState
{
    protected float input;
    public PlayerGroundState(Player player, PlayerSateMachine stateMachine, PlayerCore playerCore, string animBoolName) : base(player, stateMachine, playerCore, animBoolName)
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
        input = player.playerinput.horizontalInput;
    }


    public override void PLayerPhysics()
    {
        base.PLayerPhysics();
    }
}
