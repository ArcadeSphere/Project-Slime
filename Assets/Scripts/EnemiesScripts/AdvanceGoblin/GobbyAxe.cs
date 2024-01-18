using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GobbyAxe : MonoBehaviour
{
    [Header("Goblin Detection Settings")]
    private Animator anim;
    private Transform playerTransform;
    [SerializeField] private Vector2 chaseDetectorSize = Vector2.one;
    public Vector2 chaseDetectorOriginOffset = Vector2.zero;
    public float attackDetectionRange = 1.5f;
    public Transform chaseDetectionZoneOrigin;
    [SerializeField] private float detectionDelayDuration = 1.0f;
    private float detectionDelayTimer;
    [SerializeField] private LayerMask groundLayer;

    [Header("Reference Settings")]
    [SerializeField] private EnemyController characterFlip;

    [Header("Goblin ChaseAttack Settings")]
    [SerializeField] private float chaseSpeed = 5f;
    [SerializeField] private float stopDistance = 1.5f;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private AudioClip attackSound;
    [SerializeField] private float attackCooldown = 2f;
    private bool isCooldown = false;
    [SerializeField] private Transform attackRangeTransform;

    [Header("Goblin Patrol Settings")]
    public Transform patrolPoint1;
    public Transform patrolPoint2;
    [SerializeField] private float patrolSpeed = 3f;
    [SerializeField] private float patrolStopDuration = 2f;
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

        if (characterFlip.isFacingRight && !isTurning)
        {
            transform.Translate(Vector2.right * patrolSpeed * Time.deltaTime);
            if (transform.position.x > patrolPoint2.position.x)
            {
                isTurning = true;
                StartCoroutine(TurnDelay());
            }
        }
        else if (!characterFlip.isFacingRight && !isTurning)
        {
            transform.Translate(Vector2.left * patrolSpeed * Time.deltaTime);
            if (transform.position.x < patrolPoint1.position.x)
            {
                isTurning = true;
                StartCoroutine(TurnDelay());
            }
        }

        if (isTurning)
        {
            anim.SetFloat("moveSpeed", 0f);
        }
        else if (IsPlayerInChaseDetectionZone())
        {
            currentState = GobbyAxeState.DetectionDelay;
            anim.SetFloat("moveSpeed", 0f);
            detectionDelayTimer = detectionDelayDuration;
        }
        else
        {
            anim.SetFloat("moveSpeed", 1f);
        }

        if (!isTurning)
        {
            isTurning = false;
        }
    }

    private IEnumerator TurnDelay()
    {
        anim.SetFloat("moveSpeed", 1f);

        yield return new WaitForSeconds(patrolStopDuration);

        isTurning = false;
        characterFlip.Flip();
    }

    private void DetectionDelay()
    {
  
        detectionDelayTimer -= Time.deltaTime;

        if (detectionDelayTimer <= 0f)
        {
            if (IsPlayerInChaseDetectionZone())
            {
             
                currentState = GobbyAxeState.Chase;
            }
            else
            {
               
                currentState = GobbyAxeState.Patrol;
            }

            detectionDelayTimer = 0f;
        }
    }
    private void ChasePlayer()
    {

        Vector2 directionToPlayer = playerTransform.position - transform.position;
        directionToPlayer.Normalize();

        characterFlip.FlipTowards(playerTransform.position);

        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        // Check if there's an edge in front
        if (IsNoGroundInFront())
        {
            Debug.Log("No ground in front, returning to Patrol");
            currentState = GobbyAxeState.Patrol;
            anim.SetFloat("moveSpeed", 0f);
           

            return;
        }

        // If the player is still in the chase detection zone, continue chasing
        if (IsPlayerInChaseDetectionZone())
        {
            if (distanceToPlayer > stopDistance)
            {
                MoveTowardsPlayer(directionToPlayer);
                anim.SetFloat("moveSpeed", 1f);
            }
            else if (distanceToPlayer <= stopDistance && !IsPlayerInAttackRange(distanceToPlayer))
            {
                // Player is close but not in attack range, continue chasing
                anim.SetFloat("moveSpeed", 1f);
            }

            if (IsPlayerInAttackRange(distanceToPlayer))
            {
                // Player is in attack range, stop and attack
                StopAndAttack();
                return;
            }
        }
        else
        {
            // If the player is not in the detection zone, go back to Patrol state
           
            currentState = GobbyAxeState.Patrol;
            anim.SetFloat("moveSpeed", 0f);

        }
    }

    //check whether ground is near or not
    private bool IsNoGroundInFront()
    {
        Vector2 rayDirection = Vector2.down;
        RaycastHit2D hit = Physics2D.Raycast(edgeDetector.position, rayDirection, edgeDetectionDistance, groundLayer);

        if (hit.collider != null)
        {
            return false;
        }

        return true;
    }

    private void MoveTowardsPlayer(Vector2 direction)
    {
        transform.Translate(direction * chaseSpeed * Time.deltaTime);
    }

    private void StopAndAttack()
    {
        anim.SetFloat("moveSpeed", 0f);
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
       
        Vector2 offset = characterFlip.isFacingRight ? chaseDetectorOriginOffset : new Vector2(-chaseDetectorOriginOffset.x, chaseDetectorOriginOffset.y);
        Vector2 detectionZonePosition = (Vector2)chaseDetectionZoneOrigin.position + offset;
        Collider2D collider = Physics2D.OverlapBox(detectionZonePosition, new Vector2(chaseDetectorSize.x, chaseDetectorSize.y), 0f, playerLayer);
        //if ground is not detected the player also is not detected
        bool noGroundInFront = IsNoGroundInFront();

        return collider != null && !noGroundInFront;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 offset = characterFlip.isFacingRight ? chaseDetectorOriginOffset : new Vector2(-chaseDetectorOriginOffset.x, chaseDetectorOriginOffset.y);
        Gizmos.DrawWireCube(chaseDetectionZoneOrigin.position + offset, new Vector3(chaseDetectorSize.x * (characterFlip.isFacingRight ? 1 : -1), chaseDetectorSize.y, 1f));

        Gizmos.color = Color.blue;

     
        Gizmos.color = Color.green;
        Vector2 edgeDown = Vector2.down;
        Gizmos.DrawRay(edgeDetector.position, edgeDown * edgeDetectionDistance);

        Gizmos.color = Color.yellow;

        if (attackRangeTransform != null)
        {
            Gizmos.DrawWireSphere(attackRangeTransform.position, attackDetectionRange);
        }
    }

}