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
    public PlayerDashState dashState { get; private set; }
    public PlayerWallSlideState wallSlideState { get; private set; }
    public PlayerWallJump wallJumpState { get; private set; }


    public Animator playerAnim { get; private set; }
    public Rigidbody2D playerRb { get; private set; }

    public int FlipDirection { get; private set; } 

    public PlayerInputHandler playerinput { get; private set; }
    public float currentVelocity { get; set; }

    #endregion


    [SerializeField] private PlayerCore playerCore;

    private Vector2 workSpace;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallCheck;

    #region Awake Start Updates
    private void Awake()
    {
        stateMachine = new PlayerSateMachine();
        idleState = new PlayerIdleState(this, stateMachine, playerCore, "idle");
        moveState = new PlayerMoveState(this, stateMachine, playerCore, "move");
        jumpState = new PlayerJumpState(this, stateMachine, playerCore, "air");
        airState =  new PlayerAirState(this, stateMachine, playerCore, "air");
        landState = new PlayerLandState(this, stateMachine, playerCore, "land");
        dashState = new PlayerDashState(this, stateMachine, playerCore, "dash");
        wallSlideState = new PlayerWallSlideState(this, stateMachine, playerCore, "wallslide");
        wallJumpState = new PlayerWallJump(this, stateMachine, playerCore, "air");

    }

    private void Start()
    {
        playerAnim = GetComponent<Animator>();
        playerinput = GetComponent<PlayerInputHandler>();
        playerRb = GetComponent<Rigidbody2D>();
        stateMachine.PlayerInitialize(idleState);
        FlipDirection = 1;

    }

    private void Update()
    {
        playerinput.OnMoveInput();
        playerinput.OnJumpInput();
        playerinput.OnDashInput();
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

    public void SetDashVelocity(Vector2 velocity)
    {
        playerRb.velocity = velocity;
    }
    public void WallJumpVelocity(float velocity, Vector2 angle)
    {
        angle.Normalize();
        int wallJumpDirection = -(int)Mathf.Sign(transform.localScale.x);
        workSpace.Set(angle.x * velocity * wallJumpDirection, angle.y * velocity);
        playerRb.velocity = workSpace;
    }
    public bool CheckForGround()
    {
        return Physics2D.OverlapCircle(groundCheck.position, playerCore.GroundCheckRadius, playerCore.GroundLayer);
    }
    public bool CheckForWalls()
    {
        int facingDirection = (int)Mathf.Sign(transform.localScale.x);
        Vector2 castDirection = new Vector2(facingDirection, 0);
        RaycastHit2D hit = Physics2D.Raycast(wallCheck.position, castDirection, playerCore.WallCheckRadius, playerCore.WallLayer);

        return hit.collider != null;
    }
    public bool CheckForWallBack()
    {
        int facingDirection = -(int)Mathf.Sign(transform.localScale.x);
        Vector2 castDirection = new Vector2(facingDirection, 0);
        RaycastHit2D hit = Physics2D.Raycast(wallCheck.position, castDirection, playerCore.WallCheckRadius, playerCore.WallLayer);

        return hit.collider != null;
    }
    private void Playerflip()
    {
        Vector3 newScale = transform.localScale;
        newScale.x *= -1;

        transform.localScale = newScale;
    }
    public void PlayerShouldFlip(int xInput)
    {
         if (xInput != 0 && Mathf.Sign(xInput) != Mathf.Sign(transform.localScale.x))
        {
            Playerflip();
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