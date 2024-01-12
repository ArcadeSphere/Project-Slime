using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public bool isFacingRight = true;
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
}
