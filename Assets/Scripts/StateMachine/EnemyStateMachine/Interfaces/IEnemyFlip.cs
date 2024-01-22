using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyFlip
{
    GameObject Player { get; set; }
    SpriteRenderer SpriteRenderer { get; set; }

    bool IsFacingRight { get; set; }

    void Flip();
    void FlipTowardsPlayer();
    void FlipTowards(Vector2 targetPosition);
    void FlipOnVelocity();
    float RightX(Vector2 scale);
    float LeftX(Vector2 scale);
}
