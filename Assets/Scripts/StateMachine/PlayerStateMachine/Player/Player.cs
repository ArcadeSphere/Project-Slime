using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
public class Player : MonoBehaviour
{
    #region State
    public PlayerSateMachine stateMachine { get; private set; }
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerAirState airState { get; private set; }
    public PlayerLandState landState { get; private set; }
    public Animator playerAnim { get; private set; }
    public Rigidbody2D playerRb { get; private set; }
    public bool facingDirection { get; private set; }
    public PlayerInputHandler playerinput { get; private set; }
    public float currentVelocity { get; set; }

    #endregion


    [SerializeField] private PlayerCore playerCore;

    private Vector2 workSpace;
    [SerializeField] private Transform groundCheck;


    #region Awake Start Updates
    private void Awake()
    {
        stateMachine = new PlayerSateMachine();
        idleState = new PlayerIdleState(this, stateMachine, playerCore, "idle");
        moveState = new PlayerMoveState(this, stateMachine, playerCore, "move");
        jumpState = new PlayerJumpState(this, stateMachine, playerCore, "air");
        airState = new PlayerAirState(this, stateMachine, playerCore, "air");
        landState = new PlayerLandState(this, stateMachine, playerCore, "land");

    }

    private void Start()
    {
        playerAnim = GetComponent<Animator>();
        playerinput = GetComponent<PlayerInputHandler>();
        playerRb = GetComponent<Rigidbody2D>();
        stateMachine.PlayerInitialize(idleState);

    }

    private void Update()
    {
        playerinput.OnMoveInput();
        playerinput.OnJumpInput();
        stateMachine.CurrentState.PLayerLogic();
    }

    private void FixedUpdate()
    {
        stateMachine.CurrentState.PLayerPhysics();
    }
    #endregion

    public void SetVelocity(float velocity)
    {
        workSpace.Set(velocity, playerRb.velocity.y);
        playerRb.velocity = workSpace;
    }
    public void SetJumpVelocity(float velocity)
    {
        workSpace.Set(playerRb.velocity.x, velocity);
        playerRb.velocity = workSpace;
    }

    public bool CheckForGround()
    {
        return Physics2D.OverlapCircle(groundCheck.position, playerCore.GroundCheckRadius, playerCore.GroundLayer);
    }
    public void Playerflip()
    {
        if ((facingDirection && currentVelocity > 0f) || (!facingDirection && currentVelocity < 0f))
        {
            Vector3 localScale = transform.localScale;
            facingDirection = !facingDirection;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private void AnimationTriggerFunction()
    {
        stateMachine.CurrentState.AnimationTrigger();
    }
    private void AnimationFinishFunction()
    {
        stateMachine.CurrentState.AnimationFinish();
    }
 
}