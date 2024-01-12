using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinDetectionRange : PlayerDetector
{
    public RangeGoblin rangeGoblin;

    private void Update()
    {
        if (PlayerDetected)
        {
            rangeGoblin.ShootPlayer();
        }
        else
        {
            if (rangeGoblin.IsRangePatrolling)
            {
                rangeGoblin.RangePatrol();
            }
            else
            {
                rangeGoblin.DontShootPlayer();
            }
        }
    }
}