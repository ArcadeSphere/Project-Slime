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
        if (HasLineOfSightToPlayer())
        {
            Vector2 directionToPlayer = (player.position - firePoint.position).normalized;
            GameObject newProjectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
            EnemyProjectiles enemyProjectile = newProjectile.GetComponent<EnemyProjectiles>();
            enemyProjectile.SetSpeed(projectileSpeed);
            enemyProjectile.SetDirection(directionToPlayer);
        }

    }
    private bool HasLineOfSightToPlayer()
    {
        Vector2 directionToPlayer = playerObject.transform.position - lOS.position;

        // Use OverlapCircle to check for triggers within the radius
        Collider2D hitCollider = Physics2D.OverlapCircle(lOS.position, maxLOSRange, LayerMask.GetMask("Player"));

        if (hitCollider != null && hitCollider.CompareTag("Player"))
        {
            Debug.Log("Player detected with OverlapCircle!");
            return true;
        }
        else
        {
            Debug.Log("No trigger detected within the LOS range.");
        }

        return false;
    }

    private void OnDrawGizmos()
    {

        Gizmos.color = Color.white;
        // Draw the ray in the Scene view with a limited range only when in Detect state
        if (playerDetector.PlayerDetected)
        {
            Gizmos.DrawLine(lOS.position, lOS.position + (playerObject.transform.position - lOS.position).normalized * maxLOSRange);
        }

    }

}





