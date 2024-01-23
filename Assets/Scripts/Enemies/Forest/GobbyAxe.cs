using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GobbyAxe : MonoBehaviour
{
    [Header("Goblin Detection Settings")]
    private Animator anim;
    private Transform playerTransform;

    [SerializeField]
    private Vector2 chaseDetectorSize = Vector2.one;
    public Vector2 chaseDetectorOriginOffset = Vector2.zero;
    public float attackDetectionRange = 1.5f;
    public Transform chaseDetectionZoneOrigin;

    [SerializeField]
    private float detectionDelayDuration = 1.0f;
    private float detectionDelayTimer;

    [SerializeField]
    private LayerMask groundLayer;

    [Header("Reference Settings")]
    [SerializeField]
    private EnemyController characterFlip;

    [Header("Goblin ChaseAttack Settings")]
    [SerializeField]
    private float chaseSpeed = 5f;

    [SerializeField]
    private float stopDistance = 1.5f;

    [SerializeField]
    private LayerMask playerLayer;

    [SerializeField]
    private AudioClip attackSound;

    [SerializeField]
    private float attackCooldown = 2f;
    private bool isCooldown = false;

    [SerializeField]
    private Transform attackRangeTransform;

    [Header("Goblin Patrol Settings")]
    public Transform patrolPoint1;
    public Transform patrolPoint2;

    [SerializeField]
    private float patrolSpeed = 3f;

    [SerializeField]
    private float patrolStopDuration = 2f;
    private bool isTurning = false;

    [Header("Edge Detection Settings")]
    public Transform edgeDetector;
    public float edgeDetectionDistance = 0.2f;

    // Gobby different states
    private enum GobbyAxeState
    {
        Patrol,
        DetectionDelay,
        Chase,
        Attack,
        Cooldown
    }

    private GobbyAxeState currentState = GobbyAxeState.Patrol;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            playerTransform = playerObject.transform;
        }
    }

    private void Update()
    {
        switch (currentState)
        {
            case GobbyAxeState.Patrol:
                Patrol();
                break;

            case GobbyAxeState.DetectionDelay:
                DetectionDelay();
                break;

            case GobbyAxeState.Chase:
                ChasePlayer();
                break;

            case GobbyAxeState.Attack:
                Attack();
                break;

            case GobbyAxeState.Cooldown:
                Cooldown();
                break;
        }
    }

    private void Patrol()
    {
        if (!isTurning)
        {
            MovePatrol();
            CheckPlayerDetection();
        }
        else
        {
            HandleTurning();
        }
    }

    private void MovePatrol()
    {
        if (characterFlip.isFacingRight)
        {
            transform.Translate(Vector2.right * patrolSpeed * Time.deltaTime);
            CheckPatrolPoint(patrolPoint2.position.x);
        }
        else
        {
            transform.Translate(Vector2.left * patrolSpeed * Time.deltaTime);
            CheckPatrolPoint(patrolPoint1.position.x);
        }
    }

    private void CheckPatrolPoint(float targetPointX)
    {
        if (ShouldTurn(targetPointX))
        {
            StartTurnDelay();
        }
    }

    private bool ShouldTurn(float targetPointX)
    {
        return (characterFlip.isFacingRight && transform.position.x > targetPointX)
            || (!characterFlip.isFacingRight && transform.position.x < targetPointX);
    }

    private void StartTurnDelay()
    {
        isTurning = true;
        StartCoroutine(TurnDelay());
    }

    private IEnumerator TurnDelay()
    {
        anim.SetInteger("state", 1);
        yield return new WaitForSeconds(patrolStopDuration);
        isTurning = false;
        characterFlip.Flip();
    }

    private void HandleTurning()
    {
        anim.SetInteger("state", 0);
        if (!IsPlayerInChaseDetectionZone())
        {
            currentState = GobbyAxeState.Patrol;
        }
    }

    private void CheckPlayerDetection()
    {
        if (IsPlayerInChaseDetectionZone())
        {
            currentState = GobbyAxeState.DetectionDelay;
            anim.SetInteger("state", 0);
            detectionDelayTimer = detectionDelayDuration;
        }
        else
        {
            anim.SetInteger("state", 1);
        }
    }

    private void DetectionDelay()
    {
        detectionDelayTimer -= Time.deltaTime;
        if (detectionDelayTimer <= 0f)
        {
            currentState = IsPlayerInChaseDetectionZone()
                ? GobbyAxeState.Chase
                : GobbyAxeState.Patrol;
            detectionDelayTimer = 0f;
        }
    }

    private void ChasePlayer()
    {
        Vector2 directionToPlayer = playerTransform.position - transform.position;
        directionToPlayer.Normalize();
        characterFlip.FlipTowards(playerTransform.position);

        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        if (IsNoGroundInFront())
        {
            currentState = GobbyAxeState.Patrol;
            anim.SetInteger("state", 0);
            return;
        }

        if (IsPlayerInChaseDetectionZone())
        {
            if (distanceToPlayer > stopDistance)
            {
                MoveTowardsPlayer(directionToPlayer);
                anim.SetInteger("state", 1);
            }
            else if (distanceToPlayer <= stopDistance && !IsPlayerInAttackRange(distanceToPlayer))
            {
                anim.SetInteger("state", 1);
            }

            if (IsPlayerInAttackRange(distanceToPlayer))
            {
                StopAndAttack();
                return;
            }
        }
        else
        {
            currentState = GobbyAxeState.Patrol;
            anim.SetInteger("state", 0);
        }
    }

    private bool IsNoGroundInFront()
    {
        Vector2 rayDirection = Vector2.down;
        RaycastHit2D hit = Physics2D.Raycast(
            edgeDetector.position,
            rayDirection,
            edgeDetectionDistance,
            groundLayer
        );
        return hit.collider == null;
    }

    private void MoveTowardsPlayer(Vector2 direction)
    {
        transform.Translate(direction * chaseSpeed * Time.deltaTime);
    }

    private void StopAndAttack()
    {
        anim.SetInteger("state", 0);
        transform.Translate(Vector2.zero);
        currentState = GobbyAxeState.Attack;
    }

    private bool IsPlayerInAttackRange(float distanceToPlayer)
    {
        return distanceToPlayer < attackDetectionRange;
    }

    private void Attack()
    {
        anim.SetTrigger("AttackPlayer");
        AudioManager.instance.PlaySoundEffects(attackSound);
        currentState = GobbyAxeState.Cooldown;
        isCooldown = true;
        Invoke("EndCooldown", attackCooldown);
    }

    private void Cooldown()
    {
        if (!isCooldown)
        {
            currentState = GobbyAxeState.Patrol;
        }
    }

    private void EndCooldown()
    {
        isCooldown = false;
    }

    private bool IsPlayerInChaseDetectionZone()
    {
        Vector2 offset = characterFlip.isFacingRight
            ? chaseDetectorOriginOffset
            : new Vector2(-chaseDetectorOriginOffset.x, chaseDetectorOriginOffset.y);
        Vector2 detectionZonePosition = (Vector2)chaseDetectionZoneOrigin.position + offset;
        Collider2D collider = Physics2D.OverlapBox(
            detectionZonePosition,
            chaseDetectorSize,
            0f,
            playerLayer
        );
        bool noGroundInFront = IsNoGroundInFront();
        return collider != null && !noGroundInFront;
    }

    private void OnDrawGizmos()
    {
        DrawChaseDetectionZone();
        DrawEdgeDetectionRay();
        DrawAttackRangeSphere();
    }

    private void DrawChaseDetectionZone()
    {
        Gizmos.color = Color.red;
        Vector3 offset = characterFlip.isFacingRight
            ? chaseDetectorOriginOffset
            : new Vector2(-chaseDetectorOriginOffset.x, chaseDetectorOriginOffset.y);
        Gizmos.DrawWireCube(
            chaseDetectionZoneOrigin.position + offset,
            new Vector3(
                chaseDetectorSize.x * (characterFlip.isFacingRight ? 1 : -1),
                chaseDetectorSize.y,
                1f
            )
        );
    }

    private void DrawEdgeDetectionRay()
    {
        Gizmos.color = Color.green;
        Vector2 edgeDown = Vector2.down;
        Gizmos.DrawRay(edgeDetector.position, edgeDown * edgeDetectionDistance);
    }

    private void DrawAttackRangeSphere()
    {
        Gizmos.color = Color.yellow;
        if (attackRangeTransform != null)
        {
            Gizmos.DrawWireSphere(attackRangeTransform.position, attackDetectionRange);
        }
    }
}
