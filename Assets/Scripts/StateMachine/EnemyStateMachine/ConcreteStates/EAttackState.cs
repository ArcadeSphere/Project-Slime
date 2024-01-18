using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EAttackState : EnemyState
{
    public EAttackState(Enemy enemy, EStateMachine eStateMachine)
        : base(enemy, eStateMachine) { }

    public override void EnterState()
    {
        base.EnterState();
        enemy.Anim.SetInteger("state", 1);
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        if (!enemy.PlayerDetector.PlayerDetected)
        {
            enemy.StateMachine.ChangeState(enemy.IdleState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        enemy.GroundPatrol();
    }
}
