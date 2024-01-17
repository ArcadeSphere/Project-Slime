using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GobiHood : MonoBehaviour
{
    [Header("Reference settings")]
    [SerializeField] private PlayerDetector playerDetector;
    [SerializeField] private EnemyController characterFlip;
    [SerializeField] private EnemyPatrol enemyPatrol;

    [Header("GobiHood settings")]
    private Animator anim;
    private Rigidbody2D rb;
    [SerializeField] private float detectionDelayTimer = 0.5f;
    [SerializeField] private float shootCooldownTimer = 0.9f;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileSpeed = 5f;
    private bool isCooldown = false;


    private enum GobiHoodStates
    {
        Patrol,
        Detect,
        Shoot,
        Cooldown,
    }

    private GobiHoodStates currentStates = GobiHoodStates.Patrol;
    private void Awake()
    {
        characterFlip = GetComponent<EnemyController>();
        playerDetector = GetComponent<PlayerDetector>();
        anim = GetComponent<Animator>();

    }

    // Update is called once per frame
    private void Update()
    {
        switch (currentStates)
        {
            case GobiHoodStates.Patrol:
                Patrol();
                break;

            case GobiHoodStates.Detect:
                DetectionDelay();
                break;

            case GobiHoodStates.Shoot:
                ShootAttack();
                break;

            case GobiHoodStates.Cooldown:
                Cooldown();
                break;
        }
    }

    private void Patrol()
    {
        if (playerDetector.PlayerDetected)
        {
            Vector3 directionToPlayer = playerDetector.Target.transform.position - transform.position;
            Vector2 velocity = directionToPlayer.normalized;
            Rigidbody2D gobiRigidbody = GetComponent<Rigidbody2D>();
            gobiRigidbody.velocity = velocity;

            // Check if GobiHood is already facing the correct direction
            if ((gobiRigidbody.velocity.x > 0 && transform.localScale.x > 0) ||
                (gobiRigidbody.velocity.x < 0 && transform.localScale.x < 0))
            {
                characterFlip.FlipOnVelocity(gobiRigidbody);
                playerDetector.FlipDetector();
            }
            currentStates = GobiHoodStates.Detect;
            anim.SetFloat("moveSpeed", 0f);
            Debug.Log("Player detected, transitioning to Detect state");
        }
        else
        {
            Rigidbody2D gobiRigidbody = GetComponent<Rigidbody2D>();

            // Check if GobiHood is already facing the correct direction
            if ((gobiRigidbody.velocity.x > 0 && transform.localScale.x > 0) ||
                (gobiRigidbody.velocity.x < 0 && transform.localScale.x < 0))
            {
                characterFlip.FlipOnVelocity(gobiRigidbody);
                playerDetector.FlipDetector();
            }

            gobiRigidbody.velocity = Vector2.zero;
            enemyPatrol.GroundEnemyPatrol();
            anim.SetFloat("moveSpeed", 1f);
        }
    }

    private void DetectionDelay()
    {
        detectionDelayTimer -= Time.deltaTime;

        if (detectionDelayTimer <= 0f)
        {
            detectionDelayTimer = 0.5f;
            currentStates = GobiHoodStates.Shoot;
            Debug.Log("Detection delay over, transitioning to Shoot state");
        }

        anim.SetFloat("moveSpeed", 0f);
        enemyPatrol.StopPatrol();
    }

    private void ShootAttack()
    {
        anim.SetTrigger("Shoot");
        currentStates = GobiHoodStates.Cooldown;
        isCooldown = true;
        Invoke("EndCooldown", shootCooldownTimer);
    }

    private void Cooldown()
    {
        if (!isCooldown)
        {
            currentStates = GobiHoodStates.Patrol;

        }
    }
    private void EndCooldown()
    {
        isCooldown = false;
    }


    public void GobiShootAtPlayer()
    {
     
        Vector2 shootDirection = transform.localScale.x > 0 ? Vector2.right : Vector2.left;

        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        EnemyProjectiles projectileComponent = projectile.GetComponent<EnemyProjectiles>();

        if (projectileComponent != null)
        {
            projectileComponent.SetDirection(shootDirection);
            projectileComponent.SetSpeed(projectileSpeed);
        }
    }

}
