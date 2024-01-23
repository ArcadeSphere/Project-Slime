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

}
