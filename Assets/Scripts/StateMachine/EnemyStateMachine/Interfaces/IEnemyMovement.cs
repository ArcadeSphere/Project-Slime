using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyMovement
{
    PlayerDetector PlayerDetector { get; set; }
    Rigidbody2D RB { get; set; }
    GameObject[] PatrolPoints { get; set; }
    int CurrentPoint { get; set; }
    float TurnBackDelay { get; set; }
    float MoveSpeed { get; set; }
    bool FlipDetectorAfterTurn { get; set; }
    bool OnEdge { get; set; }
    void GroundPatrol();
    void FlyPatrol();
    IEnumerator UpdatePatrolPoints(Vector2 pointA, Vector2 pointB);
    IEnumerator PatrolEdgeDelay();
    void StopPatrol();
}
