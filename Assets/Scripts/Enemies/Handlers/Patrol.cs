using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    private Enemy enemy;

    [HideInInspector]
    public bool OnEdge;
    public float PatrolSpeed;

    public GameObject[] PatrolPoints;

    [SerializeField]
    private float turnBackDelay;

    [SerializeField]
    private bool flipDetectorAfterTurn;
    private int currentPoint;

    private void Start()
    {
        enemy = GetComponent<Enemy>();
    }

    public void FlyPatrol()
    {
        if (!OnEdge)
        {
            StartCoroutine(
                UpdatePatrolPoints(
                    PatrolPoints[currentPoint].transform.position,
                    transform.position
                )
            );
            Vector2 direction = PatrolPoints[currentPoint].transform.position - transform.position;
            enemy.RB.velocity = direction.normalized * PatrolSpeed;
        }
        else
        {
            enemy.RB.velocity = Vector2.zero;
            StartCoroutine(PatrolEdgeDelay());
        }
    }

    public void GroundPatrol()
    {
        if (!OnEdge)
        {
            StartCoroutine(
                UpdatePatrolPoints(
                    new Vector2(
                        PatrolPoints[currentPoint].transform.position.x,
                        transform.position.y
                    ),
                    transform.position
                )
            );
            Vector3 targetPosition = new Vector3(
                PatrolPoints[currentPoint].transform.position.x,
                transform.position.y,
                transform.position.z
            );
            enemy.RB.velocity = (targetPosition - transform.position).normalized * PatrolSpeed;
        }
        else
            StartCoroutine(PatrolEdgeDelay());
    }

    public IEnumerator PatrolEdgeDelay()
    {
        if (OnEdge)
        {
            yield return new WaitForSeconds(turnBackDelay);
            OnEdge = false;
        }
    }

    public void StopPatrol()
    {
        StopAllCoroutines();
        enemy.RB.velocity = Vector2.zero;
        OnEdge = false;
    }

    public IEnumerator UpdatePatrolPoints(Vector2 pointA, Vector2 pointB)
    {
        if (Vector2.Distance(pointA, pointB) < 0.1f)
        {
            currentPoint++;
            if (currentPoint >= PatrolPoints.Length)
            {
                currentPoint = 0;
            }
            // flip sprite if its the first and last point
            if (currentPoint <= 1 || currentPoint >= PatrolPoints.Length)
            {
                OnEdge = true;
                if (!flipDetectorAfterTurn)
                    enemy.PlayerDetector.FlipDetector();
                yield return new WaitForSeconds(turnBackDelay);
                if (flipDetectorAfterTurn)
                    enemy.PlayerDetector.FlipDetector();
                enemy.Flip();
            }
        }
    }
}
