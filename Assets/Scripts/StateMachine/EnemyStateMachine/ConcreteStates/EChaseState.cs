using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EChaseState : EnemyState
{
    public EChaseState(Enemy enemy, EStateMachine eStateMachine)
        : base(enemy, eStateMachine) { }

    public override void EnterState()
    {
        base.EnterState();
        enemy.EChaseBaseInstance.EnterStateLogic();
    }

    public override void ExitState()
    {
        base.ExitState();
        enemy.EChaseBaseInstance.ExitStateLogic();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        enemy.EChaseBaseInstance.FrameUpdateLogic();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        enemy.EChaseBaseInstance.PhysicsUpdateLogic();
    }

    public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
        enemy.EChaseBaseInstance.AnimationTriggerEventLogic(triggerType);
    }
}
