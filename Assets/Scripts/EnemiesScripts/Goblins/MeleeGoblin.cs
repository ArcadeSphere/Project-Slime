using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
public class MeleeGoblin : MonoBehaviour
{
    [Header("Goblin Settings")]
    private Animator anim;
    [SerializeField] private float attackRange;
    [SerializeField] private Transform attackPoint;
    private bool isAttacking = false;
    [SerializeField] private float attackCooldown = 2f;
    private float currentCooldown = 0f;
    public LayerMask playerLayer;
    [SerializeField] private float damageAmount;
    private bool isAttackAnimationInProgress = false;
    [SerializeField] private AudioClip attackSound;
   [SerializeField] private float delaySoundInSeconds = 2.0f;

    [Header("Patrol Settings")]
    [SerializeField] private Transform leftPoint;
    [SerializeField] private Transform rightPoint;
    [SerializeField] private float patrolSpeed = 2f;
    private bool isPatrolling = true;
    [SerializeField] private float stopDuration = 2f; 
    private Coroutine stopCoroutine;
    public bool IsPatrolling { get { return isPatrolling; } }
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

  

    public void MeleePatrol()
    {
        transform.Translate(Vector2.right * patrolSpeed * Time.deltaTime);
        anim.SetFloat("moveSpeed", Mathf.Abs(patrolSpeed));

        if ((patrolSpeed > 0 && transform.position.x >= rightPoint.position.x) ||
            (patrolSpeed < 0 && transform.position.x <= leftPoint.position.x))
        {
            StopPatrolForDuration();
          
        }
    }

    public void StopPatrolAndAttack()
    {
        isPatrolling = false;
        anim.SetFloat("moveSpeed", 0f);

        if (!isAttacking && currentCooldown <= 0)
        {
            AttackPlayer();
        }
        else
        {
            currentCooldown -= Time.deltaTime;
        }
    }
    private IEnumerator StopAndFlipForDuration()
    {
        yield return new WaitForSeconds(stopDuration / 2f); 
        FlipGoblin(); // 

        yield return new WaitForSeconds(stopDuration / 2f); 
        isPatrolling = true;
        stopCoroutine = null;
    }
    private void StopPatrolForDuration()
    {
        isPatrolling = false;
        anim.SetFloat("moveSpeed", 0f);
        stopCoroutine ??= StartCoroutine(StopAndFlipForDuration());
    }
    private void FlipGoblin()
    {
        patrolSpeed *= -1;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
        currentCooldown = attackCooldown;
        isAttackAnimationInProgress = false;
    }

    private IEnumerator StopForDuration()
    {
        yield return new WaitForSeconds(stopDuration);
        isPatrolling = true;
        isAttacking = false;
        stopCoroutine = null;
    }
    public void AttackPlayer()
    {
        if (!isAttackAnimationInProgress)
        {
            anim.SetTrigger("AttackPlayer");
            Invoke("PlaySoundWithDelay", delaySoundInSeconds);
            isAttacking = true;
            isAttackAnimationInProgress = true;
            StartCoroutine(AttackCooldown());

           
            isPatrolling = false;

          
            if (stopCoroutine != null)
            {
                StopCoroutine(stopCoroutine);
            }
        }
    }

    public void DontAttackPlayer()
    {
        if (!isAttackAnimationInProgress)
        {
       
            isPatrolling = false;
            if (stopCoroutine == null)
            {
                stopCoroutine = StartCoroutine(StopForDuration());
            }
        }
    }
    public void DamagePlayer()
    {
        Collider2D[] hittarget = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayer);

        foreach (Collider2D target in hittarget)
        {
            target.GetComponent<Health>().TakeDamage(damageAmount);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);

        Gizmos.DrawLine(leftPoint.position, rightPoint.position);
    }
    private void PlaySoundWithDelay()
    {
        AudioManager.instance.PlaySoundEffects(attackSound);
    }
}