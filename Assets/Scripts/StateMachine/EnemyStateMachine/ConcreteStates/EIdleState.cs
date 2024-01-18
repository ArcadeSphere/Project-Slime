using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EIdleState : EnemyState
{
    public EIdleState(Enemy enemy, EStateMachine eStateMachine)
        : base(enemy, eStateMachine) { }

    public override void EnterState()
    {
        base.EnterState();
        enemy.Anim.SetInteger("state", 0);
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        if (enemy.PlayerDetector.PlayerDetected)
        {
            enemy.StateMachine.ChangeState(enemy.AttackState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        enemy.GroundPatrol();
    }
}
