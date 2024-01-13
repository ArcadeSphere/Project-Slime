using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditorInternal;
using UnityEngine;

public class Bee : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField]
    private EnemyPatrol enemyPatrol;

    [SerializeField]
    private GameObject playerGameObject;

    [SerializeField]
    private EnemyController enemyController;

    [SerializeField]
    private PlayerDetector playerDetector;

    [Header("Bee Stats")]
    // [SerializeField]
    // private float chaseLimit = 0.5f;

    // [SerializeField]
    // private float attackCooldown = 1f;

    private SpriteRenderer sprite;
    private Animator beeAnim;
    private Rigidbody2D rb;

    // private readonly float speedMultiplier = 3f;
    // private bool inAttackRange;
    // private float lastAttackTime = 0f;
    private bool attackFinished;

    private enum BeeState
    {
        Idle,
        Attack,
        Charge
    }

    private BeeState currentState = BeeState.Idle;

    // debug
    [Header("Debug")]
    [SerializeField]
    GameObject debugText;
    private DebugText dbt;

    [SerializeField]
    GameObject debugText2;
    private DebugText dbt2;

    // debug

    private void Awake()
    {
        sprite = this.GetComponent<SpriteRenderer>();
        rb = this.GetComponent<Rigidbody2D>();
        beeAnim = this.GetComponent<Animator>();
        // debug
        dbt = debugText.GetComponent<DebugText>();
        dbt2 = debugText2.GetComponent<DebugText>();
        // debug
    }

    private void Update()
    {
        // debug
        dbt.FollowParent(this.gameObject, sprite, 0f);
        dbt2.FollowParent(this.gameObject, sprite, 0.4f);
        dbt.SetText("Current State: ", currentState.ToString());
        dbt2.SetText("Point: ", null);

        // debug
        switch (currentState)
        {
            case BeeState.Idle:
                Idle();
                break;

            case BeeState.Attack:
                Attack();
                break;
            case BeeState.Charge:
                Charge();
                break;
        }
    }

    private void Charge()
    {
        int randomPoint = enemyPatrol.patrolPoints.Length - 1;
        dbt2.SetText("Point: ", randomPoint.ToString());
        Vector3 direction =
            enemyPatrol.patrolPoints[randomPoint].transform.position - transform.position;
        rb.velocity = direction * enemyPatrol.moveSpeed;
    }

    private void FixedUpdate()
    {
        // enemyController.FlipTowardsTarget(playerGameObject.transform.position);
        // enemyController.RotateTowardsPlayer(playerDetector.DirectionToPlayer);
        if (currentState == BeeState.Idle)
        {
            enemyController.FlipOnVelocity(rb);
            return;
        }
        if (currentState == BeeState.Attack)
        {
            enemyController.FlipTowardsTarget(playerGameObject.transform.position);
            return;
        }
    }

    private void Idle()
    {
        if (playerDetector.PlayerDetected)
        {
            currentState = BeeState.Attack;
            return;
        }

        beeAnim.SetInteger("state", (int)BeeState.Idle);
        enemyPatrol.FlyEnemyPatrol();
    }

    private void Attack()
    {
        if (!playerDetector.PlayerDetected)
        {
            currentState = BeeState.Idle;
            return;
        }
        if (attackFinished)
        {
            beeAnim.SetTrigger("dying");
            Destroy(this.gameObject);
            return;
        }
        rb.velocity = Vector2.zero;
        StartCoroutine(AttackCoroutine());
    }

    private void SetAttackFinished()
    {
        attackFinished = true;
    }

    private IEnumerator AttackCoroutine()
    {
        yield return new WaitForSeconds(2);
        Vector3 lastPlayerPosition = playerGameObject.transform.position - transform.position;
        if (playerDetector.PlayerDetected)
            rb.velocity = lastPlayerPosition * enemyPatrol.moveSpeed * 3;

        if (Vector2.Distance(playerGameObject.transform.position, transform.position) < 0.1f)
            currentState = BeeState.Charge;
    }

    void DestroyGameObj()
    {
        Destroy(gameObject);
    }
}
