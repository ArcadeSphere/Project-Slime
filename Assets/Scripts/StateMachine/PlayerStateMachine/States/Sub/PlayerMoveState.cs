using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundState
{
    public PlayerMoveState(Player player, PlayerSateMachine stateMachine, PlayerCore playerCore, string animBoolName) : base(player, stateMachine, playerCore, animBoolName)
    {
    }
}
