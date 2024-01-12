using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Spider : PlayerDetector
{
    [Header("Spider Settings")]
    private Animator anim;
    private Rigidbody2D spiderRb;

    [SerializeField]
    private AudioClip attackSound;

    [SerializeField]
    private Transform playerTransform;

    [Header("Spider Jump Settings")]
    [SerializeField]
    private DetectionIndicator detectionIndicator;

    [SerializeField]
    private float jumpForce = 10f;

    [SerializeField]
    private float jumpCooldown = 2f;

    [SerializeField]
    private float SpiderJumpDelay = 1.5f;
    private float nextJumpTime;

    [SerializeField]
    private float damageAmount;

    [SerializeField]
    private Transform groundCheck;

    [SerializeField]
    private LayerMask groundLayer;

    [Header("Reference Settings")]
    [SerializeField]
    private EnemyController enemyController;

    private bool canJump = true; // Added variable to control jumping

    private void Awake()
    {
        anim = GetComponent<Animator>();
        spiderRb = GetComponent<Rigidbody2D>();
        enemyController = GetComponent<EnemyController>();
        nextJumpTime = Time.time + SpiderJumpDelay;
    }

    private void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, Vector2.down, 0.5f, groundLayer);
        bool isGround = hit.collider != null;

        if (PlayerDetected)
        {
            canJump = true; // Player is detected, so spider can jump
            if (isGround && Time.time >= nextJumpTime && canJump)
            {
                enemyController.FlipTowardsTarget(playerTransform.position);
                detectionIndicator.ActivateAlert();
                StartCoroutine(DelayedSpiderJump());
                nextJumpTime = Time.time + jumpCooldown;
            }
        }
        else
        {
            canJump = false; // Player is not detected, so spider can't jump
            detectionIndicator.DeactivateAlert();
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
}
