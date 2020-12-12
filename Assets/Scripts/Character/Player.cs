using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// Require an InputManager Component
[RequireComponent(typeof(InputManager))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Shoot))]
/// <summary>
/// Handles all behaviors required to facilitate Player interaction
/// </summary>
public class Player : MonoBehaviour
{
    private Vector2 _direction;
    private Vector2 _facing;
    private InputManager input;
    private Rigidbody2D body;
    private Shoot shoot;
    private bool isShooting = false;
    [SerializeField]
    private Transform projectileSpawner;
    [SerializeField]
    private GameObject projectileObject;
    private Projectile projectileComponent;
    [SerializeField]
    private float speed;
    private bool syncFacingOnMove = true; // Assume this must be done until the Player proves otherwise, by using a gamepad

    // Start is called before the first frame update
    void Start()
    {
        // We've ensured these objects will be available
        // by using the RequireComponent attribute
        input = GetComponent<InputManager>();
        body = GetComponent<Rigidbody2D>();
        shoot = GetComponent<Shoot>();
        _direction = new Vector2(0f, 0f);
        _facing = new Vector2(1f, 0f);
        projectileComponent = projectileObject.GetComponent<Projectile>();
    }

    /// <summary>
    /// Update position
    /// </summary>
    /// <remarks>
    /// Fixed Update is synchronized with the physics engine
    /// so all application of physics forces needs to be done here
    /// </remarks>
    private void FixedUpdate()
    {
        body.velocity = (_direction * speed);
        var newAngle = Mathf.Atan2(_facing.y, _facing.x) - Mathf.Atan2(transform.forward.y, transform.forward.x);
        newAngle *= Mathf.Rad2Deg;
        body.MoveRotation(newAngle);
    }

    // If you are interested in the value from the control that triggers an action,
    // you can declare a parameter of type InputValue.
    public void OnMove(InputValue value)
    {
        // Read value from control. The type depends on what type of controls.
        // the action is bound to.
        var v = value.Get<Vector2>();

        // IMPORTANT: The given InputValue is only valid for the duration of the callback.
        //            Storing the InputValue references somewhere and calling Get<T>()
        //            later does not work correctly.
        _direction = v;
        if (syncFacingOnMove)
        {
            var mousePos = Mouse.current.position.ReadValue();
            var mouseScreenPos = Camera.main.ScreenToWorldPoint(mousePos);
            var oldFacing = _facing;
            _facing = mouseScreenPos - transform.position;
            _facing.Normalize();
        }
    }

    public void OnLook(InputValue value)
    {
        var v = value.Get<Vector2>();
        if (v.magnitude != 0)
            _facing = v.normalized;
        syncFacingOnMove = false;
    }

    public void OnMouseMove(InputValue value)
    {
        Vector2 v = value.Get<Vector2>();
        v = Camera.main.ScreenToWorldPoint(v);
        v.x -= transform.position.x;
        v.y -= transform.position.y;
        if (v.magnitude != 0)
            _facing = v.normalized;
        syncFacingOnMove = true;
    }

    public void OnFire()
    {
        if (!isShooting)
        {
            isShooting = true;
            shoot.Fire(transform.right, projectileSpawner.position, projectileObject);
            Invoke("ResetShoot", projectileComponent.FireDelay);
        }
    }

    private void ResetShoot()
    {
        isShooting = false;
    }
}
