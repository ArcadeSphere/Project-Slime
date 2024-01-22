using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IdleFlyPatrol", menuName = "SOLogic/Idle/IdleFlyPatrol")]
public class IdleFlyPatrol : EIdleSOBase
{
    private Patrol patrol;

    public override void Init(GameObject gameObject, Enemy enemy)
    {
        base.Init(gameObject, enemy);
        patrol = enemy.GetComponent<Patrol>();
    }

    public override void EnterStateLogic()
    {
        base.EnterStateLogic();
        enemy.SetAnim(0);
    }

    public override void ExitStateLogic()
    {
        base.ExitStateLogic();
        ResetValues();
    }

    public override void FrameUpdateLogic()
    {
        base.FrameUpdateLogic();
        if (enemy.PlayerDetector.PlayerDetected)
            enemy.StateMachine.ChangeState(enemy.AttackState);
    }

    public override void PhysicsUpdateLogic()
    {
        base.PhysicsUpdateLogic();
        patrol.FlyPatrol();
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
