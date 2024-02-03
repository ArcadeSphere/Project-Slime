using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="newPlayerCore", menuName ="Core/Player Data/Base Data")]
public class PlayerCore : ScriptableObject
{
    [Header("Movement State")]
    public float MovementSpeed = 5f;


    [Header("Jump State")]
    public float JumpVelocity = 15f;
    public float jumpHeightMultiplier = 0.5f;
    public int AmountOfJumps = 1;
    public float coyoteTime = 0.2f;
    public float GroundCheckRadius = 0.2f;
    public LayerMask GroundLayer;

    [Header("Wall State")]
    public LayerMask WallLayer;
    public float WallCheckRadius = 0.4f;
    public float wallSlideForce = 3f;
    public float WallJumpPower = 20f;
    public float WallJumpTimer = 0.4f;
    public Vector2 WallJumpAnlge = new Vector2(1f, 1.5f);

    [Header("Dash State")]
    public float DashCooldown = 0.2f;
    public float DashSpeed = 24f;
    public float DashTime = 0.2f;
}
