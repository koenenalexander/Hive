using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Require an InputManager Component
[RequireComponent(typeof(InputManager))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Shoot))]
/// <summary>
/// Handles all behaviors required to facilitate Player interaction
/// </summary>
public class Player : MonoBehaviour
{
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
        body.position += (input.CommandedMovement * speed);
    }
}
