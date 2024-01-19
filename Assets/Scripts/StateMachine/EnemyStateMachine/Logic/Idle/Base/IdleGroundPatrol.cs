using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IdleGroundPatrol", menuName = "SOLogic/Idle/IdleGroundPatrol")]
public class IdleGroundPatrol : EIdleSOBase
{
    public override void Init(GameObject gameObject, Enemy enemy)
    {
        base.Init(gameObject, enemy);
    }

    public override void EnterStateLogic()
    {
        base.EnterStateLogic();
        enemy.Anim.SetInteger("state", 0);
    }

    public override void ExitStateLogic()
    {
        base.ExitStateLogic();
        ResetValues();
    }

    public override void FrameUpdateLogic()
    {
        base.FrameUpdateLogic();
        if (enemy.PlayerDetector.PlayerDetected && !enemy.OnEdge)
        {
            enemy.StateMachine.ChangeState(enemy.AttackState);
            return;
        }

        if (enemy.OnEdge)
            enemy.Anim.SetInteger("state", 0);
        else
            enemy.Anim.SetInteger("state", 1);
    }

    public override void PhysicsUpdateLogic()
    {
        base.PhysicsUpdateLogic();
        enemy.GroundPatrol();
    }

    public override void AnimationTriggerEventLogic(Enemy.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEventLogic(triggerType);
    }

    public override void ResetValues()
    {
        base.ResetValues();
    }
}
