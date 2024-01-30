using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "Charge", menuName = "SOLogic/Attack/Charge")]
public class Charge : EAttackSOBase
{
    private Patrol patrol;

    [SerializeField]
    private float attackDelay = 1f;

    [SerializeField]
    private float chargeSpeed = 4f;

    [Header("⚠️ Increase min distance if charge speed is increased")]
    [SerializeField]
    private float minDistance = 0.3f;
    private GameObject attackPoint;
    private Transform patrolPointTransform;
    private Vector2 playerLastPosition;
    private Vector2 directionToPlayerLastPosition;
    private Vector2 directionToRandomPoint;
    private int randomPatrolPoint;

    private bool isCharging = false;
    private bool isRepositioning = false;

    public override void Init(GameObject gameObject, Enemy enemy)
    {
        base.Init(gameObject, enemy);
        patrol = enemy.GetComponent<Patrol>();
        if (transform.GetChild(0).TryGetComponent<Transform>(out var attackPointTransform))
            attackPoint = attackPointTransform.gameObject;
    }

    public override void EnterStateLogic()
    {
        base.EnterStateLogic();
        SetVelocity(Vector2.zero);
        attackPoint.SetActive(true);
        enemy.StartCoroutine(AttackDelayCoroutine());
    }

    public override void ExitStateLogic()
    {
        base.ExitStateLogic();
        ResetValues();
        attackPoint.SetActive(false);
    }

    public override void FrameUpdateLogic()
    {
        base.FrameUpdateLogic();
        // distance checks
        if (isCharging)
        {
            if (Vector2.Distance(playerLastPosition, transform.position) < minDistance)
                Reposition();
        }
        else if (isRepositioning)
        {
            if (Vector2.Distance(patrolPointTransform.position, transform.position) < minDistance)
                enemy.StateMachine.ChangeState(enemy.IdleState);
        }
    }

    public override void PhysicsUpdateLogic()
    {
        base.PhysicsUpdateLogic();
        enemy.FlipTowardsPlayer();
    }

    public override void AnimationTriggerEventLogic(Enemy.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEventLogic(triggerType);
    }

    public override void ResetValues()
    {
        base.ResetValues();
        playerLastPosition = Vector2.zero;
        directionToPlayerLastPosition = Vector2.zero;
        isCharging = false;
        isRepositioning = false;
        randomPatrolPoint = 0;
    }

    private IEnumerator AttackDelayCoroutine()
    {
        yield return new WaitForSeconds(attackDelay);
        ChargeTowardsPlayer();
    }

    private void ChargeTowardsPlayer()
    {
        isCharging = true;
        // set player last position and get direction
        if (playerLastPosition == Vector2.zero)
        {
            playerLastPosition = playerTransform.position;
            directionToPlayerLastPosition = (
                playerLastPosition - (Vector2)transform.position
            ).normalized;
        }
        SetVelocity(chargeSpeed * directionToPlayerLastPosition);
    }

    private void Reposition()
    {
        isCharging = false;
        isRepositioning = true;
        // get random point from patrol points and set direction to the point
        if (randomPatrolPoint == 0)
        {
            randomPatrolPoint = Random.Range(0, patrol.PatrolPoints.Length);
            patrolPointTransform = patrol.PatrolPoints[randomPatrolPoint].transform;
            directionToRandomPoint = (
                (Vector2)patrolPointTransform.position - (Vector2)transform.position
            ).normalized;
        }
        SetVelocity(chargeSpeed * directionToRandomPoint);
    }

    // move towards the velocity vector supplied / can be Vector2.zero to stop in position
    private void SetVelocity(Vector2 velocity)
    {
        enemy.RB.velocity = velocity;
    }
}
