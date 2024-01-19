using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
public class GobiHood : MonoBehaviour
{
    [Header("Reference Settings")]
    [SerializeField] private PlayerDetector playerDetector;
    [SerializeField] private EnemyController characterController;
    [SerializeField] private EnemyPatrol enemyPatrol;

    [Header("GobiHood Settings")]
    private Animator anim;
    private bool isCooldown = false;

    [Header("GobiHood Shooting Settings")]
    [SerializeField] private float detectionDelayTimer = 0.5f;
    [SerializeField] private float shootCooldownTimer = 0.9f;
    [SerializeField] private Transform firePoint;
    [SerializeField] private Transform lineOfSight;
    [SerializeField] private Transform player;
    [SerializeField] private GameObject playerObject;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileSpeed = 5f;

    private enum GobiHoodStates
    {
        Patrol,
        Detect,
        Shoot,
        Cooldown,
    }

    private GobiHoodStates currentState = GobiHoodStates.Patrol;

    private void Awake()
    {
       
        characterController = GetComponent<EnemyController>();
        playerDetector = GetComponent<PlayerDetector>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
    
        switch (currentState)
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
            currentState = GobiHoodStates.Detect;
            anim.SetFloat("moveSpeed", 0f);
            lineOfSight.gameObject.SetActive(true);
        }
        else
        {
            lineOfSight.gameObject.SetActive(false);
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
            currentState = GobiHoodStates.Shoot;
        }

        anim.SetFloat("moveSpeed", 0f);
        enemyPatrol.StopPatrol();
    }

    private void ShootAttack()
    {
        anim.SetTrigger("Shoot");
        currentState = GobiHoodStates.Cooldown;
        isCooldown = true;
        Invoke("EndCooldown", shootCooldownTimer);
    }

    private void Cooldown()
    {
        if (!isCooldown)
        {
            currentState = GobiHoodStates.Patrol;
        }
    }

    private void EndCooldown()
    {
        isCooldown = false;
    }

    public void GobiShootAtPlayer()
    {
        Vector2 directionToPlayer = (player.position - firePoint.position).normalized;

        float dotProduct = Vector2.Dot(directionToPlayer, lineOfSight.right);

        //arrow track the player
        if ((dotProduct > 0 && characterController.isFacingRight) || (dotProduct < 0 && !characterController.isFacingRight))
        {
            CreateProjectile(directionToPlayer);
        }
        //doesnt track the player
        else
        {
            CreateProjectile(characterController.isFacingRight ? Vector2.right : Vector2.left);
        }
    }

    private void CreateProjectile(Vector2 direction)
    {
        GameObject newProjectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        EnemyProjectiles enemyProjectile = newProjectile.GetComponent<EnemyProjectiles>();
        enemyProjectile.SetSpeed(projectileSpeed);
        enemyProjectile.SetDirection(direction);
    }
}




