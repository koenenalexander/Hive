using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static VectorMath;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Shoot))]
[RequireComponent(typeof(DamageHandler))]
public class Enemy : MonoBehaviour
{
    private const string PLAYER = "Player";
    private const string ENEMY = "Enemy";

    private Vector3 startFacing;
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
    [SerializeField]
    private float speed = 2f;
    private List<Enemy> allies = new List<Enemy>();
    private Enemy leader = null;
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
    private DamageHandler damageHandler;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        shoot = GetComponent<Shoot>();
        mover = GetComponent<Movement>();
        mover.SetSpeed(speed);
        projectileComponent = projectileObject.GetComponent<Projectile>();
        startFacing = transform.right;
        damageHandler = GetComponent<DamageHandler>();
        damageHandler.AddHandler(Damage);
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            transform.right = target.position - transform.position;
        }
        else if (body.velocity.magnitude > 0.001)
        {
            transform.right = body.velocity.normalized;
        }
        else
            transform.right = startFacing;

        switch (state)
        {
            case ENEMY_STATE.Idle:
                if (!HasTarget)
                    SetTarget(FindPlayer());
                else if (TargetLost())
                {
                    mover.StopMoving();
                    leader = null;
                    target = null;
                }
                else
                {
                    state = ENEMY_STATE.Attacking;
                    mover.Follow(target, sightRange / 2);
                    Invoke("FireAtPlayer", projectileComponent.FireDelay);
                }
                break;
            case ENEMY_STATE.Attacking:
                break;
            default:
                break;
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

    private void SetTarget(Transform transform, Enemy caller = null)
    {
        if (transform != null)
        {
            target = transform;
            CalloutTarget(transform);

            // Make the target caller the leader
            if (caller != null && leader == null)
                leader = caller;
        }
    }

    private void CalloutTarget(Transform transform)
    {
        foreach (var ally in allies)
        {
            if (!ally.HasTarget)
                ally.SetTarget(transform, this);
        }
    }

    private void CallForHelp()
    {
        foreach (var ally in allies)
        {
            if (!ally.HasTarget)
                ally.SearchLocation(transform.position);
        }
    }
    private void SearchLocation(Vector3 pos)
    {
        mover.Search(pos, sightRange);
    }

    private bool TargetLost()
    {
        var difference = target.position - transform.position;
        if (leader != null)
            difference = target.position - leader.transform.position;
        return difference.magnitude > sightRange * 2;
    }

    private void Damage(int damage)
    {
        CallForHelp();
        Destroy(gameObject);
    }
}
