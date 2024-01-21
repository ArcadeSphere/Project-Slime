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

    [Header("Bee variables")]
    [SerializeField]
    private float chargeSpeed = 3f;

    [SerializeField]
    private float maxChargeDelay = 1f;

    [SerializeField]
    private float minChargeDelay = 0.4f;
    private float chargeDelay = 0f;
    private GameObject attackPoint;
    private SpriteRenderer sprite;
    private Animator beeAnim;
    private Rigidbody2D rb;
    private int randomPoint = 0;
    private Vector3 direction;
    private bool isAttackFinished;
    private bool isAttacking;
    private Vector3 directionToLastPlayerPosition = Vector3.zero;
    private Vector3 lastPlayerPosition = Vector3.zero;

    private BeeState currentState = BeeState.Idle;

    private enum BeeState
    {
        Idle,
        Attack,
        Reposition
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
        if (transform.GetChild(0).TryGetComponent<Transform>(out var attackPointTransform))
            attackPoint = attackPointTransform.gameObject;
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
            case BeeState.Reposition:
                Reposition();
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
        {
            attackPoint.SetActive(false);
            GoToRandomPatrolPoint();
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
        // sets isAttacking to true if player is detected
        // it allows bee to charge towards player even if player left the detector after being detected once
        if (playerDetector.PlayerDetected)
        {
            isAttacking = true;
        }
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

    private void Reposition()
    {
        // debug
        dbt2.SetText("Point: ", randomPoint.ToString());

        // if true bee goes back to patrol position
        isAttackFinished = true;
    }

    private IEnumerator AttackCoroutine()
    {
        if (chargeDelay == 0f)
        {
            chargeDelay = Random.Range(minChargeDelay, maxChargeDelay);
        }
        // pause bee for chargeDelay seconds
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(chargeDelay);
        attackPoint.SetActive(true);
        // then charge towards player
        // set player's position as last player position
        if (lastPlayerPosition == Vector3.zero)
        {
            lastPlayerPosition = playerGameObject.transform.position;
        }
        directionToLastPlayerPosition = lastPlayerPosition - transform.position;
        rb.velocity = chargeSpeed * enemyPatrol.moveSpeed * directionToLastPlayerPosition;
        // exit case if bee is close to player
        if (Vector2.Distance(lastPlayerPosition, transform.position) < 0.1f)
        {
            currentState = BeeState.Reposition;
            isAttacking = false;
        }
    }

    private void GoToRandomPatrolPoint()
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
            ResetChargeValues();
        }
    }

    private void ResetChargeValues()
    {
        directionToLastPlayerPosition = Vector3.zero;
        lastPlayerPosition = Vector3.zero;
        chargeDelay = 0f;
    }

    private void DestroyGameObj()
    {
        Destroy(gameObject);
    }
}
