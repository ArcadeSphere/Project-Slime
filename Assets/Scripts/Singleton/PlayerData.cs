using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : Singleton<PlayerData>
{
    [HideInInspector]
    public Transform currentCheckpoint;

    [HideInInspector]
    public bool isPlayerDead = false;

    [HideInInspector]
    public Vector2 previousPosition;

    [HideInInspector]
    public bool isFacingRight;
}
