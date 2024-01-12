using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
public class GoblinDetection : PlayerDetector
{
    public MeleeGoblin meleeGoblin;

    private void Update()
    {
        if (PlayerDetected)
        {
            meleeGoblin.StopPatrolAndAttack();
        }
        else
        {
            if (meleeGoblin.IsPatrolling)
            {
                meleeGoblin.MeleePatrol();
            }
            else
            {
                meleeGoblin.DontAttackPlayer();
            }
        }
    }
}
