using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerGroundState
{
    public PlayerIdleState(Player player, PlayerSateMachine stateMachine, PlayerCore playerCore, string animBoolName) : base(player, stateMachine, playerCore, animBoolName)
    {
    }

  
}
