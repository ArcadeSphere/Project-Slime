using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour, IEnemyMovement, IEnemyFlip
{
    [HideInInspector]
    public Animator Anim;

    #region Patrol Variables
    public PlayerDetector PlayerDetector { get; set; }
    public Rigidbody2D RB { get; set; }

    [field: SerializeField]
    public GameObject[] PatrolPoints { get; set; }
    public int CurrentPoint { get; set; }

    [field: SerializeField]
    public float TurnBackDelay { get; set; }

    [field: SerializeField]
    public float MoveSpeed { get; set; }

    [field: SerializeField]
    public bool FlipDetectorAfterTurn { get; set; }
    public bool OnEdge { get; set; }
    #endregion

    #region Flip Variables
    public GameObject Player { get; set; }
    public SpriteRenderer SpriteRenderer { get; set; }

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
    [field: SerializeField]
    private EIdleSOBase EnemyIdleBase;

    [field: SerializeField]
    private EChaseSOBase EnemyChaseBase;

    [field: SerializeField]
    private EAttackSOBase EnemyAttackBase;

    public EIdleSOBase EIdleBaseInstance { get; set; }

    public EChaseSOBase EChaseBaseInstance { get; set; }

    public EAttackSOBase EAttackBaseInstance { get; set; }
    #endregion

    #region Debug
    // debug
    [Header("Debug")]
    [SerializeField]
    private GameObject dt;
    private DebugText dbt;
    private TextMeshPro tmp;
    #endregion

    private void Awake()
    {
        EIdleBaseInstance = Instantiate(EnemyIdleBase);
        EChaseBaseInstance = Instantiate(EnemyChaseBase);
        EAttackBaseInstance = Instantiate(EnemyAttackBase);

        StateMachine = gameObject.AddComponent<EStateMachine>();

        IdleState = new EIdleState(this, StateMachine);
        ChaseState = new EChaseState(this, StateMachine);
        AttackState = new EAttackState(this, StateMachine);
        // debug
        if (dt != null)
        {
            dbt = dt.GetComponent<DebugText>();
        }
        // debug
    }

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        SpriteRenderer = GetComponent<SpriteRenderer>();
        RB = GetComponent<Rigidbody2D>();
        PlayerDetector = GetComponent<PlayerDetector>();
        Anim = GetComponent<Animator>();

        EIdleBaseInstance.Init(gameObject, this);
        EChaseBaseInstance.Init(gameObject, this);
        EAttackBaseInstance.Init(gameObject, this);

        StateMachine.InitializeState(IdleState);
    }

    private void Update()
    {
        StateMachine.CurrentEnemyState.FrameUpdate();
        // debug
        if (dbt != null)
        {
            dbt.FollowParent(gameObject, SpriteRenderer);
            dbt.SetText("Current State: ", StateMachine.CurrentEnemyState.ToString());
        }
        // debug
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentEnemyState.PhysicsUpdate();
    }

    #region Animation Trigger
    public enum AnimationTriggerType
    {
        EnemyHurt,
        EnemyAttack,
        EnemyDie
    }

    private void AnimationTriggerEvent(AnimationTriggerType triggerType)
    {
        StateMachine.CurrentEnemyState.AnimationTriggerEvent(triggerType);
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

    #region Patrol Functions
    public void FlyPatrol()
    {
        if (!OnEdge)
        {
            StartCoroutine(
                UpdatePatrolPoints(
                    PatrolPoints[CurrentPoint].transform.position,
                    transform.position
                )
            );
            Vector2 direction = PatrolPoints[CurrentPoint].transform.position - transform.position;
            RB.velocity = direction.normalized * MoveSpeed;
        }
        else
        {
            RB.velocity = Vector2.zero;
            StartCoroutine(PatrolEdgeDelay());
        }
    }

    public void GroundPatrol()
    {
        if (!OnEdge)
        {
            StartCoroutine(
                UpdatePatrolPoints(
                    new Vector2(
                        PatrolPoints[CurrentPoint].transform.position.x,
                        transform.position.y
                    ),
                    transform.position
                )
            );
            Vector3 targetPosition = new Vector3(
                PatrolPoints[CurrentPoint].transform.position.x,
                transform.position.y,
                transform.position.z
            );
            RB.velocity = (targetPosition - transform.position).normalized * MoveSpeed;
        }
        else
            StartCoroutine(PatrolEdgeDelay());
    }

    public IEnumerator PatrolEdgeDelay()
    {
        if (OnEdge)
        {
            yield return new WaitForSeconds(TurnBackDelay);
            OnEdge = false;
        }
    }

    public void StopPatrol()
    {
        StopAllCoroutines();
        RB.velocity = Vector2.zero;
        OnEdge = false;
    }

    public IEnumerator UpdatePatrolPoints(Vector2 pointA, Vector2 pointB)
    {
        if (Vector2.Distance(pointA, pointB) < 0.1f)
        {
            CurrentPoint++;
            if (CurrentPoint >= PatrolPoints.Length)
            {
                CurrentPoint = 0;
            }
            // flip sprite if its the first and last point
            if (CurrentPoint <= 1 || CurrentPoint >= PatrolPoints.Length)
            {
                OnEdge = true;
                if (!FlipDetectorAfterTurn)
                    PlayerDetector.FlipDetector();
                yield return new WaitForSeconds(TurnBackDelay);
                if (FlipDetectorAfterTurn)
                    PlayerDetector.FlipDetector();
                Flip();
            }
        }
    }
    #endregion
}
