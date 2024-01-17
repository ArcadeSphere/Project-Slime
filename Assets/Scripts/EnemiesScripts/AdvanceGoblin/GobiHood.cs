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
    [SerializeField] private float detectionDelayTimer = 0.5f;
    [SerializeField] private float shootCooldownTimer = 0.9f;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileSpeed = 5f;
    private bool isShooting = false;


    private enum GobiHoodStates
    {
        Patrol,
        Detect,
        Shoot,

     
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
                if (!isShooting) // Only enter the Shoot state if not currently shooting
                {
                    Debug.Log("Entering Shoot state");
                    ShootAttack();
                }
                break;
        }
    }

    private void Patrol()
    {
        if (playerDetector.PlayerDetected)
        {
            isShooting = false; // Reset the shooting state
            currentStates = GobiHoodStates.Detect;
            anim.SetFloat("moveSpeed", 0f); // Stop animation when detecting the player
            Debug.Log("Player detected, transitioning to Detect state");
        }
        else
        {
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
        isShooting = true;

        if (shootCooldownTimer > 0f)
        {
            shootCooldownTimer -= Time.deltaTime;
        }
        else
        {
            isShooting = false; // Reset the shooting state

            if (playerDetector.PlayerDetected)
            {
                // Player is still detected, continue shooting
                shootCooldownTimer = 5f;
                Debug.Log("Player still detected, continuing to shoot");
            }
            else
            {
                // Player is not detected, transition back to Patrol
                currentStates = GobiHoodStates.Patrol;
                Debug.Log("Player not detected, transitioning back to Patrol state");
            }
        }
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
