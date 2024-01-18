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
        enemy.EIdleBaseInstance.EnterStateLogic();
    }

    public override void ExitState()
    {
        base.ExitState();
        enemy.EIdleBaseInstance.ExitStateLogic();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        enemy.EIdleBaseInstance.FrameUpdateLogic();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        enemy.EIdleBaseInstance.PhysicsUpdateLogic();
    }

    public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
        enemy.EIdleBaseInstance.AnimationTriggerEventLogic(triggerType);
    }
}
