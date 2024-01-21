using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
public class RangeGoblin : MonoBehaviour
{
    [Header("Goblin Settings")]
    private Animator anim;
    private bool isShooting = false;
    [SerializeField] private float shootingCooldown = 2f;
    private bool isShootAnimationInProgress = false;
    private float currentCooldown = 0f;

    [Header("Arrow Settings")]
    public GameObject arrowPrefab;
    [SerializeField] private Transform firePoint;
    public float arrowSpeed = 5f;
    [SerializeField] private AudioClip arrowSound;
    [SerializeField] private float delaySoundInSeconds = 2.0f;
    [SerializeField] private float delayBeforeShooting = 0.5f; 
    [SerializeField] private float shootingAnimationDuration = 1.5f;

    [Header("Patrol Settings")]
    [SerializeField] private Transform leftPoint;
    [SerializeField] private Transform rightPoint;
    [SerializeField] private float patrolSpeed = 2f;
    private bool isPatrolling = true;
    [SerializeField] private float stopDuration = 2f;
    private Coroutine stopCoroutine;
    public bool IsRangePatrolling { get { return isPatrolling; } }
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    //patrol 
    public void RangePatrol()
    {
        transform.Translate(Vector2.right * patrolSpeed * Time.deltaTime);
        anim.SetFloat("moveSpeed", Mathf.Abs(patrolSpeed));

        // Check if the goblin is beyond the patrol range
        if ((patrolSpeed > 0 && transform.position.x >= rightPoint.position.x) ||
            (patrolSpeed < 0 && transform.position.x <= leftPoint.position.x))
        {
            // Flip the goblin to change direction
            FlipGoblin();
        }
    }
    public void StopPatrolAndRangeAttack()
    {
        isPatrolling = false;
        anim.SetFloat("moveSpeed", 0f);

        if (!isShooting && currentCooldown <= 0)
        {
            ShootPlayer();
        }
        else
        {
            currentCooldown -= Time.deltaTime;
        }
    }
 
    //flip when reaching end
    private void FlipGoblin()
    {
        patrolSpeed *= -1;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
    //cooldown for enemy shooting
    private IEnumerator ShootingCooldown()
    {
        yield return new WaitForSeconds(shootingCooldown);
        isShooting = false;
        currentCooldown = shootingCooldown;
        isShootAnimationInProgress = false;
    }


    private IEnumerator StopForDuration()
    {
        yield return new WaitForSeconds(stopDuration);
        isPatrolling = true;
        isShooting = false;
        stopCoroutine = null;
    }
    private void StopRangePatrolForDuration()
    {
        isPatrolling = false;
        anim.SetFloat("moveSpeed", 0f);

        if (stopCoroutine == null)
        {
            stopCoroutine = StartCoroutine(StopRangeForDuration());
        }
    }

    private IEnumerator StopRangeForDuration()
    {
        yield return new WaitForSeconds(stopDuration / 2f);

        // Comment out or remove the FlipGoblin call to prevent turning around
        // FlipGoblin(); 

        yield return new WaitForSeconds(stopDuration / 2f);
        isPatrolling = true;
        stopCoroutine = null;
    }
    //use in animator to shoot
    public void ShootPlayer()
    {
        if (!isShootAnimationInProgress && isPatrolling)
        {
            // Stop patrolling immediately
            isPatrolling = false;
            anim.SetFloat("moveSpeed", 0f);

            if (stopCoroutine != null)
            {
                StopCoroutine(stopCoroutine);
            }

            // Start the shooting sequence
            StartCoroutine(StartShootingSequence());
        }
    }
    private IEnumerator StartShootingSequence()
    {
        // Wait for a short delay if needed
        yield return new WaitForSeconds(delayBeforeShooting);

        // Start shooting animation
        anim.SetTrigger("Shooting");
        Invoke("PlaySoundWithDelay", delaySoundInSeconds);

        // Wait for the shooting animation to finish
        yield return new WaitForSeconds(shootingAnimationDuration);

        // Start shooting cooldown
        isShootAnimationInProgress = true;
        isShooting = true;
        StartCoroutine(ShootingCooldown());

        // Resume patrolling after shooting
        isPatrolling = true;
        stopCoroutine = StartCoroutine(StopRangeForDuration());
    }
    public void DontShootPlayer()
    {
        if (!isShootAnimationInProgress)
        {

            isPatrolling = false;
            if (stopCoroutine == null)
            {
                stopCoroutine = StartCoroutine(StopForDuration());
            }
        }
    }
    void OnDrawGizmosSelected()
    {
    
        Gizmos.DrawLine(leftPoint.position, rightPoint.position);
    }
    public void ShootArrows()
    {
        GameObject projectile = Instantiate(arrowPrefab, firePoint.position, firePoint.rotation);

       ArrowProjectiles projectileComponent = projectile.GetComponent<ArrowProjectiles>();
        if (projectileComponent != null)
        {
            Vector2 shootDirection = (transform.localScale.x > 0) ? Vector2.right : Vector2.left;
            projectileComponent.SetSpeed(arrowSpeed);
            projectileComponent.SetProjectileDirection(shootDirection);
        }
    }
    public void ResumePatrol()
    {
        if (!isPatrolling)
        {
            isPatrolling = true;
            anim.SetFloat("moveSpeed", Mathf.Abs(patrolSpeed));

            if (stopCoroutine != null)
            {
                StopCoroutine(stopCoroutine);
                stopCoroutine = null;
            }
        }
    }
    private void PlaySoundWithDelay()
    {
        AudioManager.instance.PlaySoundEffects(arrowSound);
    }
}
