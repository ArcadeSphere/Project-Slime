using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] GameObject[] waypoints;
    [SerializeField] private float moveSpeed = 3f;
    private int currentPointIndex = 0;

    private void Update() 
    {
        MovePlatform();
    }

    void MovePlatform()
    {
        if (Vector2.Distance(waypoints[currentPointIndex].transform.position, transform.position) < 0.1f)
        {
            currentPointIndex++;
            if (currentPointIndex >= waypoints.Length)
            {
                currentPointIndex = 0;
            }
        }
        transform.position = Vector2.MoveTowards(transform.position, waypoints[currentPointIndex].transform.position, moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.CompareTag("Player"))
        {
            collision.gameObject.transform.SetParent(transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        collision.gameObject.transform.SetParent(null);
    }
}
