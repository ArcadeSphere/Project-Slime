using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
   public PlayerSateMachine stateMachine { get; private set; }

   public PlayerIdleState idleState { get; private set; }

   public PlayerMoveState moveState { get; private set; }


    public Animator playerAnim { get; private set; }
    [SerializeField] private PlayerCore playerCore;
    private void Awake()
    {
        stateMachine = new PlayerSateMachine();

        idleState = new PlayerIdleState(this, stateMachine, playerCore, "idle");
        moveState = new PlayerMoveState(this, stateMachine, playerCore, "move");


    }
    private void Start()
    {
        playerAnim = GetComponent<Animator>();
        stateMachine.PlayerInitialize(idleState);
    }
    private void Update()
    {
        stateMachine.CurrentState.PLayerLogic();
    }
    private void FixedUpdate()
    {
        stateMachine.CurrentState.PLayerPhysics();
    }
}
