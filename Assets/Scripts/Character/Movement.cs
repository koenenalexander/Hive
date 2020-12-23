using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static VectorMath;

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
    private float searchRadius = 0;
    private bool withinSearchRange = false;
    private Rigidbody2D body;
    private void Start()
    {
        startPos = transform.position;
        body = GetComponent<Rigidbody2D>();
    }
    public void Follow(Transform target, float range = 0.0f)
    {
        this.target = target;
        keepAtDistance = range;
    }
    public void SetSpeed(float speed)
    {
        maxSpeed = speed;
    }
    public void StopMoving()
    {
        this.target = null;
    }

    public void Search(Vector3 searchPos, float radius)
    {
        targetSearchPos = searchPos;
        searchRadius = radius;
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
            body.velocity = GetSearchVector(targetSearchPos, transform.position, searchRadius);
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

    private Vector2 GetSearchVector(
        Vector3 searchPos, 
        Vector3 targetPos,
        float radius)
    {
        Vector2 searchVector = Vector2.zero;
        var deltaPos = searchPos - targetPos;
        bool withinRange = deltaPos.magnitude < searchRadius ||
            (withinSearchRange && deltaPos.magnitude < searchRadius * 1.2f);
        if (withinRange)
        {
            searchVector = RotateBy90(deltaPos);
        }
        else
        {
            searchVector = deltaPos.normalized * maxSpeed;
        }
        bool failedToFind = withinSearchRange && !withinRange;
        withinSearchRange = withinRange;
        if (failedToFind)
            targetSearchPos = Vector3.negativeInfinity;
        return searchVector;
    }
}
