using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private Transform target = null;
    private float keepAtDistance;
    private float maxSpeed;
    private Vector3 startPos;
    private Rigidbody2D body;

    private void Start()
    {
        startPos = transform.position;
        body = GetComponent<Rigidbody2D>();
    }
    public void Follow(Transform target, float speed, float range = 0.0f)
    {
        this.target = target;
        maxSpeed = speed;
        keepAtDistance = range;
    }

    public void StopMoving()
    {
        this.target = null;
    }

    private void FixedUpdate()
    {
        // No target, return to starting point
        if (target == null)
        {
            body.velocity = Vector2.zero;
        }
        else
        {
            Vector2 deltaPos = target.position - transform.position;
            bool withinRange = deltaPos.magnitude < keepAtDistance;
            if (withinRange)
            {
                body.velocity = Vector2.zero;
            }
            else
            {
                body.velocity = deltaPos.normalized * maxSpeed;
            }
        }
    }
}
