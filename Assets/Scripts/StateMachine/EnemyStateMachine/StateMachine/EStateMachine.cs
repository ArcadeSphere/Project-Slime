using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EStateMachine : MonoBehaviour
{
    public EnemyState CurrentEnemyState { get; set; }

    public void InitializeState(EnemyState initialState)
    {
        CurrentEnemyState = initialState;
        CurrentEnemyState.EnterState();
    }

    public void ChangeState(EnemyState newState)
    {
        CurrentEnemyState.ExitState();
        CurrentEnemyState = newState;
        CurrentEnemyState.EnterState();
    }
}
