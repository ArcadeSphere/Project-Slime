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
        enemy.EAttackBaseInstance.EnterStateLogic();
    }

    public override void ExitState()
    {
        base.ExitState();
        enemy.EAttackBaseInstance.ExitStateLogic();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        enemy.EAttackBaseInstance.FrameUpdateLogic();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        enemy.EAttackBaseInstance.PhysicsUpdateLogic();
    }

    public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
        enemy.EAttackBaseInstance.AnimationTriggerEventLogic(triggerType);
    }
}
