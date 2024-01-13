using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public bool isFacingRight = true;

    [SerializeField]
    private float rotateSpeed;

    [SerializeField]
    private float rotationModifier;
    private Vector2 scale;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        scale = transform.localScale;
    }

    //Flip the character
    public void Flip()
    {
        isFacingRight = !isFacingRight;
        scale.x *= -1;
        transform.localScale = scale;
    }

    //Flip the character according to other objects tranform
    public void FlipTowardsTarget(Vector3 targetPosition)
    {
        if (targetPosition.x > transform.position.x)
        {
            scale.x = Mathf.Abs(scale.x) * -1 * (spriteRenderer.flipX ? -1 : 1);
        }
        else
        {
            scale.x = Mathf.Abs(scale.x) * (spriteRenderer.flipX ? -1 : 1);
        }
        transform.localScale = scale;
    }

    //Keep this version also
    public void FlipTowards(Vector3 targetPos)
    {
        if (targetPos.x < transform.position.x)
        {
            if (isFacingRight)
            {
                Flip();
            }
        }
        else
        {
            if (!isFacingRight)
            {
                Flip();
            }
        }
    }

    public void FlipOnVelocity(Rigidbody2D enemyRb)
    {
        if (enemyRb.velocity.x < 0f)
        {
            scale.x = 1;
        }
        else if (enemyRb.velocity.x > 0f)
        {
            scale.x = -1;
        }
        else
        {
            scale.x *= 1;
        }
        transform.localScale = scale;
    }

    public void RotateTowardsPlayer(Vector3 directionToPlayer)
    {
        float angle =
            Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg
            - rotationModifier;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * rotateSpeed);
    }
}
