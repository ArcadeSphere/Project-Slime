using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
   public PlayerSateMachine stateMachine { get; private set; }

    private void Awake()
    {
        stateMachine = new PlayerSateMachine();

    }
    private void Start()
    {
        
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
