using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reposition : EChaseSOBase
{
    #region Dependencies
    private Patrol patrol;
    #endregion
    private float speed;
    private Transform patrolPointTransform;
    private Vector2 directionToRandomPoint;
    private int randomPatrolPoint;

    #region ChaseBase Functions
    public override void Init(GameObject gameObject, Enemy enemy)
    {
        base.Init(gameObject, enemy);
    }

    public override void EnterStateLogic()
    {
        base.EnterStateLogic();
    }

    public override void ExitStateLogic()
    {
        base.ExitStateLogic();
        ResetValues();
    }

    public override void FrameUpdateLogic()
    {
        base.FrameUpdateLogic();
    }

    public override void PhysicsUpdateLogic()
    {
        base.PhysicsUpdateLogic();
        RepositionEnemy();
    }

    public override void AnimationTriggerEventLogic(Enemy.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEventLogic(triggerType);
    }

    public override void ResetValues()
    {
        base.ResetValues();
        randomPatrolPoint = 0;
    }
    #endregion
    private void RepositionEnemy()
    {
        // get random point from patrol points and set direction to the point
        if (randomPatrolPoint == 0)
        {
            randomPatrolPoint = Random.Range(0, patrol.PatrolPoints.Length);
            patrolPointTransform = patrol.PatrolPoints[randomPatrolPoint].transform;
            directionToRandomPoint = (
                (Vector2)patrolPointTransform.position - (Vector2)transform.position
            ).normalized;
        }
        enemy.RB.velocity = speed * directionToRandomPoint;
    }
}
