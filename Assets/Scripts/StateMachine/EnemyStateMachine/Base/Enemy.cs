using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour, IEnemyFlip
{
    [HideInInspector]
    public Animator Animator;

    [HideInInspector]
    public Rigidbody2D RB;

    [HideInInspector]
    public PlayerDetector PlayerDetector;

    #region Flip Variables
    public GameObject Player { get; set; }
    public SpriteRenderer SpriteRenderer { get; set; }

    [field: Header("Flip")]
    [field: SerializeField]
    public bool IsFacingRight { get; set; }
    #endregion

    #region State Machine Variables
    public EStateMachine StateMachine { get; set; }
    public EIdleState IdleState { get; set; }
    public EChaseState ChaseState { get; set; }
    public EAttackState AttackState { get; set; }
    #endregion

    #region ScriptableObjects variables
    [Header("Behaviours / States")]
    [SerializeField]
    private EIdleSOBase EnemyIdleBase;

    [SerializeField]
    private EChaseSOBase EnemyChaseBase;

    [SerializeField]
    private EAttackSOBase EnemyAttackBase;

    public EIdleSOBase EIdleBaseInstance { get; set; }

    public EChaseSOBase EChaseBaseInstance { get; set; }

    public EAttackSOBase EAttackBaseInstance { get; set; }
    #endregion

    private void Awake()
    {
        // instancing
        EIdleBaseInstance = Instantiate(EnemyIdleBase);
        EChaseBaseInstance = Instantiate(EnemyChaseBase);
        EAttackBaseInstance = Instantiate(EnemyAttackBase);
        // adding component state machine
        StateMachine = gameObject.AddComponent<EStateMachine>();
        // creating instances
        IdleState = new EIdleState(this, StateMachine);
        ChaseState = new EChaseState(this, StateMachine);
        AttackState = new EAttackState(this, StateMachine);
    }

    private void Start()
    {
        // getting components
        Player = GameObject.FindGameObjectWithTag("Player");
        SpriteRenderer = GetComponent<SpriteRenderer>();
        RB = GetComponent<Rigidbody2D>();
        PlayerDetector = GetComponent<PlayerDetector>();
        Animator = GetComponent<Animator>();
        // initializing behaviours
        EIdleBaseInstance.Init(gameObject, this);
        EChaseBaseInstance.Init(gameObject, this);
        EAttackBaseInstance.Init(gameObject, this);
        // initializing start state
        StateMachine.InitializeState(IdleState);
    }

    private void Update()
    {
        StateMachine.CurrentEnemyState.FrameUpdate();
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentEnemyState.PhysicsUpdate();
    }

    public void SetAnim(int animHashCode)
    {
        Animator.SetInteger("state", animHashCode);
    }

    #region Animation Trigger
    public enum AnimationTriggerType
    {
        zero,
        one,
        two,
        three,
        four,
        five,
        six,
        changeState
    }

    // useful when changing states or applying extra logic
    private void AnimationTriggerEvent(AnimationTriggerType triggerType)
    {
        StateMachine.CurrentEnemyState.AnimationTriggerEvent(triggerType);
    }

    // useful when changing animation
    private void ChangeAnimationEventTrigger(AnimationTriggerType triggerType)
    {
        Animator.SetInteger("state", (int)triggerType);
    }
    #endregion

    #region Flip Functions
    public void Flip()
    {
        Vector2 scale = transform.localScale;
        IsFacingRight = !IsFacingRight;
        scale.x *= -1;
        transform.localScale = scale;
    }

    public void FlipOnVelocity()
    {
        Vector2 scale = transform.localScale;
        if (RB.velocity.x < 0f)
        {
            scale.x = 1;
        }
        else if (RB.velocity.x > 0f)
        {
            scale.x = -1;
        }
        else
        {
            scale.x *= 1;
        }
        transform.localScale = scale;
    }

    public void FlipTowards(Vector2 targetPosition)
    {
        Vector2 scale = transform.localScale;
        if (targetPosition.x < transform.position.x)
        {
            if (IsFacingRight)
            {
                Flip();
            }
        }
        else
        {
            if (!IsFacingRight)
            {
                Flip();
            }
        }
    }

    public void FlipTowardsPlayer()
    {
        Vector2 scale = transform.localScale;
        if (Player.transform.position.x > transform.position.x)
        {
            scale.x = Mathf.Abs(scale.x) * -1 * (SpriteRenderer.flipX ? -1 : 1);
        }
        else
        {
            scale.x = Mathf.Abs(scale.x) * (SpriteRenderer.flipX ? -1 : 1);
        }
        transform.localScale = scale;
    }
    #endregion
}
