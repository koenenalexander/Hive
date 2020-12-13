using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Shoot))]
public class Enemy : MonoBehaviour
{
    private Shoot shoot;
    [SerializeField]
    private Transform projectileSpawner;
    private float sightRange = 1.0f;
    private Transform target;
    [SerializeField]
    private GameObject projectileObject;
    private Projectile projectileComponent;
    private Rigidbody2D body;
    private Movement mover;
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
                if (CanSeePlayer())
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
    private bool CanSeePlayer()
    {
        bool playerSpotted = false;
        var startPos = projectileSpawner.position;
        var endPos = startPos + transform.right * sightRange;
        var ray = Physics2D.Linecast(startPos, endPos, LayerMask.GetMask("Player"));
        Debug.DrawLine(startPos, endPos, Color.red, Time.deltaTime);
        if (ray.fraction > 0)
        {
            playerSpotted = true;
            target = ray.transform;
        }
        return playerSpotted;
    }
}
