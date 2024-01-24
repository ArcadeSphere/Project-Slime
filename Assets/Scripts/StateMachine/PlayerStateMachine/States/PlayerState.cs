using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected Player player;
    protected PlayerSateMachine stateMachine;
    protected PlayerCore playerCore;
    protected bool isAnimFinish;
    protected float startTime;
    private string animBoolName;

    public PlayerState(Player player, PlayerSateMachine stateMachine ,PlayerCore playerCore, string animBoolName)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.playerCore = playerCore;
        this.animBoolName = animBoolName;
    }

    public virtual void PlayerEnterState()
    {
        player.SetVelocity(0f);
        Debug.Log("Player idle");
        CheckForSomething();
        player.playerAnim.SetBool(animBoolName, true);
        startTime = Time.time;
        isAnimFinish = false;
    }
    public virtual void PLayerExitState()
    {
        player.playerAnim.SetBool(animBoolName, false);
    }
    public virtual void PLayerLogic()
    {

    }
    public virtual void PLayerPhysics()
    {
        CheckForSomething();
    }

    public virtual void CheckForSomething()
    {

    }
    public virtual void AnimationTrigger()
    {

    }
    public virtual void AnimationFinish()
    {
        isAnimFinish = true;
    }
}
