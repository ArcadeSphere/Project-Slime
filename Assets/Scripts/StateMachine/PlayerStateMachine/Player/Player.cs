using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerSateMachine stateMachine { get; private set; }

    public PlayerIdleState idleState { get; private set; }

    public PlayerMoveState moveState { get; private set; }


    public Animator playerAnim { get; private set; }

    public Rigidbody2D playerRb { get; private set; }

    public int FacingDiretion { get; private set; }
    public PlayerInputHandler playerinput { get; private set; }

    [SerializeField] private PlayerCore playerCore;

    private Vector2 workSpace;

    private void Awake()
    {
        stateMachine = new PlayerSateMachine();

        idleState = new PlayerIdleState(this, stateMachine, playerCore, "idle");
        moveState = new PlayerMoveState(this, stateMachine, playerCore, "move");


    }
    private void Start()
    {
        playerAnim = GetComponent<Animator>();
        playerinput = GetComponent<PlayerInputHandler>();
        playerRb = GetComponent<Rigidbody2D>();
        FacingDiretion = 1;
        stateMachine.PlayerInitialize(idleState);
    }
    private void Update()
    {
        playerinput.OnMoveInput();
        stateMachine.CurrentState.PLayerLogic();
    }
    private void FixedUpdate()
    {
        stateMachine.CurrentState.PLayerPhysics();
    }
    public void setVelocity(float velocity)
    {
        workSpace.Set(velocity, playerRb.velocity.y);
        playerRb.velocity = workSpace;
    }
    private void flip()
    {
        FacingDiretion *= -1;
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }
    public void PlayerShouldFlip(float xinput)
    {
        if(xinput != 0 && xinput != FacingDiretion)
        {
            flip();
        }
    }
}
