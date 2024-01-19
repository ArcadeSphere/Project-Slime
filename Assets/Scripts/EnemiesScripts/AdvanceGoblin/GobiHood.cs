using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
public class GobiHood : MonoBehaviour
{
    [Header("Reference settings")]
    [SerializeField] private PlayerDetector playerDetector;
    [SerializeField] private EnemyController characterFlip;
    [SerializeField] private EnemyPatrol enemyPatrol;

    [Header("GobiHood settings")]
    private Animator anim;
    private bool isCooldown = false;


    [Header("GobiHood Shooting settings")]
    [SerializeField] private float detectionDelayTimer = 0.5f;
    [SerializeField] private float shootCooldownTimer = 0.9f;
    [SerializeField] private Transform firePoint;
    [SerializeField] private Transform lOS;
    [SerializeField] private Transform player;
    [SerializeField] private GameObject playerObject;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileSpeed = 5f;
    [SerializeField] private float maxLOSRange = 10f;
    [SerializeField] private float yOffset = 0.5f;
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
            currentStates = GobiHoodStates.Detect;
            anim.SetFloat("moveSpeed", 0f);
            lOS.gameObject.SetActive(true);
        }
        else
        {
            lOS.gameObject.SetActive(false);
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


    //tracks the player
    public void GobiShootAtPlayer()
    {
        Vector2 directionToPlayer = (player.position - firePoint.position).normalized;

        // Check if the player is in front of the detection zone based on Gobi's facing direction
        float dotProduct = Vector2.Dot(directionToPlayer, lOS.right);

        if ((dotProduct > 0 && characterFlip.isFacingRight) || (dotProduct < 0 && !characterFlip.isFacingRight))
        {
            GameObject newProjectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
            EnemyProjectiles enemyProjectile = newProjectile.GetComponent<EnemyProjectiles>();
            enemyProjectile.SetSpeed(projectileSpeed);
            enemyProjectile.SetDirection(directionToPlayer);
        }
        
    }


}





