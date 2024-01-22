using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Dive", menuName = "SOLogic/Attack/Dive")]
public class Dive : EAttackSOBase
{
    private event Action OnAttack;

    [SerializeField]
    private float attackDelay;

    [SerializeField]
    private float chargeSpeed = 4f;
    private GameObject attackPoint;
    private Vector2 playerLastPosition = Vector2.zero;
    private Vector2 directionToPlayerLastPosition = Vector2.zero;

    public override void Init(GameObject gameObject, Enemy enemy)
    {
        base.Init(gameObject, enemy);
        OnAttack += Attack;
        if (transform.GetChild(0).TryGetComponent<Transform>(out var attackPointTransform))
            attackPoint = attackPointTransform.gameObject;
    }

    public override void EnterStateLogic()
    {
        base.EnterStateLogic();
        enemy.RB.velocity = Vector2.zero;
        attackPoint.SetActive(true);
        enemy.StartCoroutine(AttackDelayCoroutine());
    }

    public override void ExitStateLogic()
    {
        base.ExitStateLogic();
        ResetValues();
    }

    public override void FrameUpdateLogic()
    {
        base.FrameUpdateLogic();
        if (!enemy.PlayerDetector.PlayerDetected)
            enemy.StateMachine.ChangeState(enemy.IdleState);
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
    }

    private IEnumerator AttackDelayCoroutine()
    {
        yield return new WaitForSeconds(attackDelay);
        OnAttack?.Invoke();
    }

    private void Attack()
    {
        if (playerLastPosition == Vector2.zero)
        {
            playerLastPosition = playerTransform.position;
            directionToPlayerLastPosition = playerLastPosition - (Vector2)transform.position;
        }
        enemy.RB.velocity = directionToPlayerLastPosition.normalized * chargeSpeed;
    }
}
