using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Spider : MonoBehaviour
{
    [Header("Spider Settings")]
    private Animator anim;
    private Rigidbody2D spiderRb;
    [SerializeField] private AudioClip attackSound;
    [SerializeField] private Transform playerTransform;

    [Header("Spider Jump Settings")]
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float jumpCooldown = 2f;
    [SerializeField] private float SpiderJumpDelay = 1.5f;
    private float nextJumpTime;
    [SerializeField] private float damageAmount;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;


    [Header("Reference Settings")]
    [SerializeField] private PlayerDetector playerDetector;
    [SerializeField] private EnemyController characterFlip;

    private bool canJump = true;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        spiderRb = GetComponent<Rigidbody2D>();
        characterFlip = GetComponent<EnemyController>();
        playerDetector = GetComponent<PlayerDetector>();
        nextJumpTime = Time.time + SpiderJumpDelay;
    }

    private void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, Vector2.down, 0.5f, groundLayer);
        bool isGround = hit.collider != null;

        if (playerDetector.PlayerDetected)
        {
           
            if (isGround && Time.time >= nextJumpTime && canJump)
            {
                characterFlip.FlipTowardsTarget(playerTransform.position);
                StartCoroutine(DelayedSpiderJump());
                nextJumpTime = Time.time + jumpCooldown;
            }
        }
      
    }

    // adds a delay for the first jump
    private IEnumerator DelayedSpiderJump()
    {
        yield return new WaitForSeconds(SpiderJumpDelay);
        JumpAttack();
    }

    // jump attack
    private void JumpAttack()
    {
        anim.SetTrigger("JumpAttack");
        AudioManager.instance.PlaySoundEffects(attackSound);
        float distanceFromPlayer = playerTransform.position.x - transform.position.x;
        spiderRb.AddForce(new Vector2(distanceFromPlayer, jumpForce), ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Obstacle"))
        {
            anim.SetTrigger("dying");
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        // Draw a line for the groundCheck ray cast
        Gizmos.DrawLine(groundCheck.position, groundCheck.position + Vector3.down * 0.5f);
    }
}