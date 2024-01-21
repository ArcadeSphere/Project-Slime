using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingingThing : MonoBehaviour
{
    private Rigidbody2D swtrb;
    [SerializeField] private float chainSpeed;
    [SerializeField] private float leftAngle;
    [SerializeField] private float rightAngle;
    bool ClockWiseMovement;
    void Start()
    {
        swtrb = GetComponent<Rigidbody2D>();
        
    }

  
    void Update()
    {

        ChainMovement();
    }

    public void ChainDirection() { 
    
    if(transform.rotation.z > rightAngle)
        {
            ClockWiseMovement = false;
        }
        if (transform.rotation.z < leftAngle)
        {
            ClockWiseMovement = true;
        }
    }

    public void ChainMovement()
    {
        ChainDirection();
        if (ClockWiseMovement)
        {
            swtrb.angularVelocity = chainSpeed;
        }
        if (!ClockWiseMovement)
        {
            swtrb.angularVelocity = -1 * chainSpeed;
        }
    }
}
