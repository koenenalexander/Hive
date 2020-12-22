using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Shoot))]
public class Enemy : MonoBehaviour
{
    private const string PLAYER = "Player";
    private const string ENEMY = "Enemy";

    private Shoot shoot;
    [SerializeField]
    private Transform projectileSpawner;
    private float sightRange = 1.0f;
    private Transform target;
    private bool HasTarget => target != null;
    [SerializeField]
    private GameObject projectileObject;
    private Projectile projectileComponent;
    private Rigidbody2D body;
    private Movement mover;
    [SerializeField]
    private int viewConeSize = 90; // Degrees
    private int searchAngleDelta = 15; // allows for control over precision of search for target
    private int currentSearchDelta = 0;
    private List<Enemy> allies = new List<Enemy>();
    private enum TARGET_SEARCH_DIRECTION
    {
        Clockwise,
        CounterClockwise
    }
    private TARGET_SEARCH_DIRECTION searchDirection = TARGET_SEARCH_DIRECTION.Clockwise;
    private enum ENEMY_STATE
    {
        Invalid,
        Idle,
        Attacking
    }
    private ENEMY_STATE state = ENEMY_STATE.Idle;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        shoot = GetComponent<Shoot>();
        mover = GetComponent<Movement>();
        projectileComponent = projectileObject.GetComponent<Projectile>();
    }

    // Update is called once per frame
    void Update()
    {
        switch(state)
        {
            case ENEMY_STATE.Idle:
                if (!HasTarget)
                    SetTarget(FindPlayer());
                else
                {
                    state = ENEMY_STATE.Attacking;
                    mover.Follow(target, 1f, sightRange / 2);
                    Invoke("FireAtPlayer", projectileComponent.FireDelay);
                }
                break;
            case ENEMY_STATE.Attacking:
                break;
            default:
                break;
        }
    }

    private void FixedUpdate()
    {
        if (target != null)
        {
            transform.right = target.position - transform.position;
        }
    }

    private void FireAtPlayer()
    {
        shoot.Fire(transform.right.normalized, projectileSpawner.position, projectileObject);
        Invoke("ResetFire", projectileComponent.FireDelay);
    }
    private void ResetFire()
    {
        state = ENEMY_STATE.Idle;
    }
    private Transform FindPlayer()
    {
       Transform player = null;
       var startPos = projectileSpawner.position;
       var adjustmentAngle = GetSearchAdjustmentAngle() * Mathf.Deg2Rad;
       var adjustedTarget = RotateVector2D(transform.right, adjustmentAngle);
       var endPos = startPos + adjustedTarget * sightRange;

       var ray = Physics2D.Linecast(startPos, endPos, LayerMask.GetMask("Player"));

       Debug.DrawLine(startPos, endPos, Color.red, Time.deltaTime);
       if (ray.fraction > 0)
       {
            player = ray.transform;
       }
        return player;
    }

    /// <summary>
    /// Calculates the angle by which to adjust the
    /// current search sweep through the enemy view cone
    /// </summary>
    /// <returns>Adjustment angle in degrees</returns>
    private int GetSearchAdjustmentAngle()
    {
        int maxDelta = viewConeSize / 2;
        int minDelta = (-1 * viewConeSize / 2);
        int adjustmentAngle = currentSearchDelta;
        if (searchDirection == TARGET_SEARCH_DIRECTION.Clockwise)
            adjustmentAngle += searchAngleDelta;
        else
            adjustmentAngle -= searchAngleDelta;

        if (adjustmentAngle > maxDelta)
        {
            adjustmentAngle = maxDelta;
            searchDirection = TARGET_SEARCH_DIRECTION.CounterClockwise;
        }
        else if (adjustmentAngle < minDelta)
        {
            adjustmentAngle = minDelta;
            searchDirection = TARGET_SEARCH_DIRECTION.Clockwise;
        }
        currentSearchDelta = adjustmentAngle;
        return adjustmentAngle;
    }

    /// <summary>
    /// Calculates a new Vector which is the rotation of
    /// the provided vector by the provided angle
    /// </summary>
    /// <param name="v0">Vector to Rotate</param>
    /// <param name="angle">Angle, in degrees, to rotate</param>
    /// <returns>Rotated Vector</returns>
    private Vector3 RotateVector2D(Vector3 v0, float angle)
    {
        // 2D Vector Rotation Formula
        // x1 = x0*cos(a) - y0*sin(a)
        // y1 = x0*sin(a) + y0*cos(a)
        float x1 = v0.x * Mathf.Cos(angle) - v0.y * Mathf.Sin(angle);
        float y1 = v0.x * Mathf.Sin(angle) + v0.y * Mathf.Cos(angle);

        return new Vector3(x1, y1, v0.z);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(PLAYER))
            SetTarget(collision.transform);
        else if (collision.CompareTag(ENEMY))
        {
            // Save reference to enemy for coordination manuevers
            allies.Add(collision.gameObject.GetComponent<Enemy>());
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(ENEMY))
        {
            var enemy = collision.gameObject.GetComponent<Enemy>();
            if (allies.Contains(enemy))
                allies.Remove(enemy);
        }
    }

    private void SetTarget(Transform transform)
    {
        if (transform != null)
        {
            target = transform;
            CalloutTarget(transform);
        }
    }

    private void CalloutTarget(Transform transform)
    {
        foreach (var ally in allies)
        {
            if (!ally.HasTarget)
                ally.SetTarget(transform);
        }
    }
}
