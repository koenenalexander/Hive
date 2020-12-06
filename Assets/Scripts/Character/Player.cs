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
    private InputManager input;
    private Rigidbody2D body;
    private Shoot shoot;
    [SerializeField]
    private float speed;

    // Start is called before the first frame update
    void Start()
    {
        // We've ensured these objects will be available
        // by using the RequireComponent attribute
        input = GetComponent<InputManager>();
        body = GetComponent<Rigidbody2D>();
        shoot = GetComponent<Shoot>();
        _direction = new Vector2(0f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        
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
        body.position += (_direction * speed);
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
    }

    public void OnFire()
    {
        shoot.Fire(_direction);
    }
}
