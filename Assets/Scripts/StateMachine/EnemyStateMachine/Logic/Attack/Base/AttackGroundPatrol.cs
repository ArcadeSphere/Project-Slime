using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackGroundPatrol", menuName = "SOLogic/Attack/AttackGroundPatrol")]
public class AttackGroundPatrol : EAttackSOBase
{
    private Patrol patrol;

    private readonly float initialSpeed = 2f;

    [SerializeField]
    private float speed;

    public override void Init(GameObject gameObject, Enemy enemy)
    {
        base.Init(gameObject, enemy);
        patrol = enemy.GetComponent<Patrol>();
    }

    public override void EnterStateLogic()
    {
        base.EnterStateLogic();
        enemy.Animator.SetInteger("state", Armadillo.Anim.Curl.GetHashCode());
    }

    public override void ExitStateLogic()
    {
        base.ExitStateLogic();
        ResetValues();
    }

    public override void FrameUpdateLogic()
    {
        base.FrameUpdateLogic();
        AnimatorStateInfo currentAnim = enemy.Animator.GetCurrentAnimatorStateInfo(0);
        if (!currentAnim.IsName("curl") && currentAnim.normalizedTime > 1f)
            patrol.PatrolSpeed = speed;
        if (patrol.OnEdge)
        {
            enemy.Animator.SetInteger("state", Armadillo.Anim.Uncurl.GetHashCode());
        }
    }

    public override void PhysicsUpdateLogic()
    {
        base.PhysicsUpdateLogic();
        patrol.GroundPatrol();
    }

    public override void AnimationTriggerEventLogic(Enemy.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEventLogic(triggerType);
        if (triggerType == Enemy.AnimationTriggerType.changeState)
        {
            enemy.StateMachine.ChangeState(enemy.IdleState);
        }
    }

    public override void ResetValues()
    {
        base.ResetValues();
        patrol.PatrolSpeed = initialSpeed;
    }
}
