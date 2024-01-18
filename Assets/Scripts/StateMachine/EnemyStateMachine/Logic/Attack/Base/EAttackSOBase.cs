using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EAttackSOBase : ScriptableObject
{
    protected Enemy enemy;
    protected Transform transform;
    protected GameObject gameObject;
    protected Transform playerTransform;

    public virtual void Init(GameObject gameObject, Enemy enemy)
    {
        this.gameObject = gameObject;
        this.enemy = enemy;
        this.transform = gameObject.transform;
        this.playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public virtual void EnterStateLogic() { }

    public virtual void ExitStateLogic()
    {
        ResetValues();
    }

    public virtual void FrameUpdateLogic() { }

    public virtual void PhysicsUpdateLogic() { }

    public virtual void AnimationTriggerEventLogic(Enemy.AnimationTriggerType triggerType) { }

    public virtual void ResetValues() { }
}
