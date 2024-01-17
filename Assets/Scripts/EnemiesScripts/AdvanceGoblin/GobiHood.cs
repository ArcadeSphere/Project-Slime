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
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileSpeed = 5f;
    [SerializeField] private Transform projectileSpawnPoint;
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
        projectileSpawnPoint = transform.Find("ProjectileSpawnPoint");
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

                // Flip the firePoint's local scale along with the GobiHood
                Vector3 firePointScale = firePoint.localScale;
                firePointScale.x = Mathf.Abs(firePointScale.x) * Mathf.Sign(transform.localScale.x);
                firePoint.localScale = firePointScale;

                playerDetector.FlipDetector();
            }
            currentStates = GobiHoodStates.Detect;
            anim.SetFloat("moveSpeed", 0f);
            Debug.Log("Player detected, transitioning to Detect state");
        }
        else
        {
            Rigidbody2D gobiRigidbody = GetComponent<Rigidbody2D>();

            if ((gobiRigidbody.velocity.x > 0 && transform.localScale.x > 0) ||
                (gobiRigidbody.velocity.x < 0 && transform.localScale.x < 0))
            {
                characterFlip.FlipOnVelocity(gobiRigidbody);

                // Flip the firePoint's local scale along with the GobiHood
                Vector3 firePointScale = firePoint.localScale;
                firePointScale.x = Mathf.Abs(firePointScale.x) * Mathf.Sign(transform.localScale.x);
                firePoint.localScale = firePointScale;

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
        Vector2 shootDirection = firePoint.right;

        // Check the current facing direction dynamically
        bool isGobiFacingRight = characterFlip.isFacingRight;

        Debug.Log($"Before FlipOnVelocity: isGobiFacingRight: {isGobiFacingRight}, transform.localScale.x: {transform.localScale.x}, firePoint.localScale.x: {firePoint.localScale.x}");

        // Call FlipOnVelocity before adjusting firePoint's scale
        characterFlip.FlipOnVelocity(GetComponent<Rigidbody2D>());

        if (!isGobiFacingRight)
        {
            // Flip the shootDirection if Gobi is not facing right
            shootDirection = -shootDirection;
        }

        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        EnemyProjectiles projectileComponent = projectile.GetComponent<EnemyProjectiles>();

        if (projectileComponent != null)
        {
            projectileComponent.SetDirection(shootDirection);
            projectileComponent.SetSpeed(projectileSpeed);
        }

        Debug.Log($"After FlipOnVelocity: isGobiFacingRight: {isGobiFacingRight}, transform.localScale.x: {transform.localScale.x}, firePoint.localScale.x: {firePoint.localScale.x}");
    }

}





