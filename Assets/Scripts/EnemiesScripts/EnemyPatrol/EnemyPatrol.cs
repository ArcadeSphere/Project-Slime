using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyPatrol : MonoBehaviour
{
    // public variables
    [HideInInspector]
    public bool onEdge = false; // used by PlayerDetector script to handle animations

    // private variables
    [SerializeField]
    private EnemyController enemyController;

    [SerializeField]
    private GameObject[] patrolPoints;
    private readonly float moveSpeed = 3f;

    [SerializeField]
    private float turnBackDelay;

    [SerializeField]
    private bool flipDetectorAfterTurn = false;

    private PlayerDetector playerDetector; // set if needed for specific animations

    private Rigidbody2D enemyRb;
    private int currentPoint = 0;

    private void Start()
    {
        enemyRb = this.GetComponent<Rigidbody2D>();
        playerDetector = this.GetComponent<PlayerDetector>();
    }

    // for enemies that can't fly
    public void GroundEnemyPatrol()
    {
        if (!onEdge)
        {
            StartCoroutine(
                UpdatePatrolPoints(
                    new Vector2(
                        patrolPoints[currentPoint].transform.position.x,
                        transform.position.y
                    ),
                    transform.position
                )
            );
            Vector3 targetPosition = new Vector3(
                patrolPoints[currentPoint].transform.position.x,
                transform.position.y,
                transform.position.z
            );
            enemyRb.velocity = (targetPosition - transform.position).normalized * moveSpeed;
        }
        else
            StartCoroutine(PatrolEdgeDelay());
    }

    // for enemies that fly
    public void FlyEnemyPatrol()
    {
        if (!onEdge)
        {
            StartCoroutine(
                UpdatePatrolPoints(
                    patrolPoints[currentPoint].transform.position,
                    transform.position
                )
            );
            Vector2 direction = patrolPoints[currentPoint].transform.position - transform.position;
            enemyRb.velocity = direction.normalized * moveSpeed;
        }
        else
        {
            enemyRb.velocity = Vector2.zero;
            StartCoroutine(PatrolEdgeDelay());
        }
    }

    private IEnumerator UpdatePatrolPoints(Vector2 pointA, Vector2 pointB)
    {
        if (Vector2.Distance(pointA, pointB) < 0.1f)
        {
            
            currentPoint++;
            if (currentPoint >= patrolPoints.Length)
            {
                currentPoint = 0;
            }
            // flip sprite if its the first and last point
            if (currentPoint <= 1 || currentPoint >= patrolPoints.Length)
            {
                onEdge = true;
                if (!flipDetectorAfterTurn)
                    playerDetector.FlipDetector();
                yield return new WaitForSeconds(turnBackDelay);
                if (flipDetectorAfterTurn)
                    playerDetector.FlipDetector();
                enemyController.Flip();
            }
        }
    }

    private IEnumerator PatrolEdgeDelay()
    {
        if (onEdge)
        {
            yield return new WaitForSeconds(turnBackDelay);
            onEdge = false;
        }
    }
}
