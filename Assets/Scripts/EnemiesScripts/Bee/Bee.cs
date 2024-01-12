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

    [SerializeField]
    private float rotateSpeed;

    [SerializeField]
    private float rotationModifier;

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
        Chase
    }

    private BeeState currentState = BeeState.Idle;

    // debug
    [Header("Debug")]
    [SerializeField]
    GameObject debugText;
    private DebugText dbt;

    // debug

    private void Awake()
    {
        sprite = this.GetComponent<SpriteRenderer>();
        rb = this.GetComponent<Rigidbody2D>();
        beeAnim = this.GetComponent<Animator>();
        // debug
        dbt = debugText.GetComponent<DebugText>();
        // debug
    }

    private void Update()
    {
        // debug
        dbt.FollowParent(this.gameObject, sprite);
        dbt.SetText("Current State: ", currentState.ToString());
        // debug
        switch (currentState)
        {
            case BeeState.Idle:
                Idle();
                break;

            case BeeState.Attack:
                Attack();
                break;
        }
    }

    private void FixedUpdate()
    {
        enemyController.FlipTowardsTarget(playerGameObject.transform.position);
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
            DestroyEnemy();
            return;
        }
        rb.velocity = Vector2.zero;
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
        enemyController.FlipOnVelocity(rb);
    }

    private void SetAttackFinished()
    {
        attackFinished = true;
    }

    private void RotateTowardsPlayer()
    {
        float angle =
            Mathf.Atan2(playerDetector.DirectionToPlayer.y, playerDetector.DirectionToPlayer.x)
                * Mathf.Rad2Deg
            - rotationModifier;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * rotateSpeed);
    }

    private void DestroyEnemy()
    {
        Destroy(this.gameObject);
    }
}
