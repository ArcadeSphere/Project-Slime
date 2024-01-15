using System.Collections;
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
    private SpriteRenderer sprite;
    private Animator beeAnim;
    private Rigidbody2D rb;
    private int randomPoint = 0;
    private Vector3 direction;
    private bool isAttackFinished;
    private bool isAttacking;
    private BeeState currentState = BeeState.Idle;

    private enum BeeState
    {
        Idle,
        Attack,
        Charge
    }

    [Header("Debug")]
    [SerializeField]
    GameObject debugText;

    [SerializeField]
    GameObject debugText2;
    private DebugText dbt;
    private DebugText dbt2;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        beeAnim = GetComponent<Animator>();
        // debug
        dbt = debugText.GetComponent<DebugText>();
        dbt2 = debugText2.GetComponent<DebugText>();
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

        // change flip logic based on currentState
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
        // sets isAttacking to true if player is detected
        // it allows bee to charge towards player even if player left the detector after being detected once
        if (playerDetector.PlayerDetected)
            isAttacking = true;
        if (isAttacking)
        {
            StartCoroutine(AttackCoroutine());
            return;
        }
        if (!playerDetector.PlayerDetected)
        {
            currentState = BeeState.Idle;
            return;
        }
    }

    private void Charge()
    {
        // debug
        dbt2.SetText("Point: ", randomPoint.ToString());

        // if true bee goes back to patrol position
        isAttackFinished = true;
    }

    private IEnumerator AttackCoroutine()
    {
        // pause bee for 1 second
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(1);
        // then charge towards player
        Vector2 playerPosition = playerGameObject.transform.position - transform.position;
        rb.velocity = 3 * enemyPatrol.moveSpeed * playerPosition;
        // exit case if bee is close to player
        if (Vector2.Distance(playerGameObject.transform.position, transform.position) < 0.1f)
        {
            currentState = BeeState.Charge;
            isAttacking = false;
        }
    }

    void GoToRandomPatrolPoint()
    { // set random point if it's 0
        if (randomPoint == 0)
            randomPoint = Random.Range(0, enemyPatrol.patrolPoints.Length);
        // variable that hold random position of a patrol point
        Vector3 randomPatrolPointPosition = enemyPatrol
            .patrolPoints[randomPoint]
            .transform
            .position;
        // direction to patrol point
        if (direction != null)
            direction = randomPatrolPointPosition - transform.position;
        rb.velocity = direction * enemyPatrol.moveSpeed;
        // exit case if bee is close to patrol point
        if (Vector2.Distance(randomPatrolPointPosition, transform.position) < 0.1f)
        {
            currentState = BeeState.Attack;
            isAttackFinished = false;
            randomPoint = 0;
        }
    }

    void DestroyGameObj()
    {
        Destroy(gameObject);
    }
}
