using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEditorInternal;
using UnityEngine;
using Random = UnityEngine.Random;

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
    private int randomPoint = 0;
    private Vector3 direction;

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
    private bool isAttackFinished;

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

    private void FixedUpdate()
    {
        // debug
        dbt.FollowParent(this.gameObject, sprite, 0f);
        dbt2.FollowParent(this.gameObject, sprite, 1.6f);
        dbt.SetText("Current State: ", currentState.ToString());
        dbt2.SetText("Point: ", null);

        // debug
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

    private void LateUpdate()
    {
        if (isAttackFinished)
            GoToRandomPatrolPoint();
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
        rb.velocity = Vector2.zero;
        StartCoroutine(AttackCoroutine());
    }

    private void Charge()
    {
        dbt2.SetText("Point: ", randomPoint.ToString());
        isAttackFinished = true;
    }

    void GoToRandomPatrolPoint()
    {
        if (randomPoint == 0)
            randomPoint = Random.Range(0, enemyPatrol.patrolPoints.Length);
        Vector3 randomPatrolPointPosition = enemyPatrol
            .patrolPoints[randomPoint]
            .transform
            .position;
        if (direction != null)
            direction = randomPatrolPointPosition - transform.position;
        rb.velocity = direction * enemyPatrol.moveSpeed;
        if (Vector2.Distance(randomPatrolPointPosition, transform.position) < 0.1f)
        {
            currentState = BeeState.Attack;
            isAttackFinished = false;
            randomPoint = 0;
        }
    }

    private IEnumerator AttackCoroutine()
    {
        yield return new WaitForSeconds(1);
        Vector3 lastPlayerPosition = playerGameObject.transform.position - transform.position;
        if (playerDetector.PlayerDetected)
            rb.velocity = 3 * enemyPatrol.moveSpeed * lastPlayerPosition;

        if (Vector2.Distance(playerGameObject.transform.position, transform.position) < 0.1f)
            currentState = BeeState.Charge;
    }

    void DestroyGameObj()
    {
        Destroy(gameObject);
    }
}
