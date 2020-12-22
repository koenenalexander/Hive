using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private const float MIN_DIST = 0.01f;
    private Transform target = null;
    private bool HasTarget => target != null;
    private float keepAtDistance;
    private float maxSpeed;
    private Vector3 startPos;
    private Vector3 targetSearchPos = Vector3.negativeInfinity;
    private bool HasTargetSearchPos => targetSearchPos.x > float.NegativeInfinity;
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
        if (HasTarget)
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
        else if (HasTargetSearchPos)
        {
            body.velocity = Vector2.zero;
        }
        // No target, return to starting point
        else
        {
            var returnVector = startPos - transform.position;
            if (returnVector.magnitude > MIN_DIST)
                body.velocity = returnVector.normalized * maxSpeed;
            else
                body.velocity = Vector2.zero;
        }
    }
}
