using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState
{
    protected Enemy enemy;
    protected EStateMachine eStateMachine;

    public EnemyState(Enemy enemy, EStateMachine eStateMachine)
    {
        this.enemy = enemy;
        this.eStateMachine = eStateMachine;
    }

    public virtual void EnterState() { }

    public virtual void ExitState() { }

    public virtual void FrameUpdate() { }

    public virtual void PhysicsUpdate() { }

    public virtual void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType) { }
}
