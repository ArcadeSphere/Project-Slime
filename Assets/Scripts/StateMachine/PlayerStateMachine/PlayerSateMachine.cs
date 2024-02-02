using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSateMachine
{
   public PlayerState CurrentState { get; private set; }

  public void PlayerInitialize(PlayerState startingState)
    {
        CurrentState = startingState;
        CurrentState.PlayerEnterState();
    }
    
    public void PlayerChangeState(PlayerState newState)
    {
        CurrentState.PLayerExitState();
        CurrentState = newState;
        CurrentState.PlayerEnterState();

    }
}
