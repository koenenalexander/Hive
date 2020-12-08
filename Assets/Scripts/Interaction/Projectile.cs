using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    private float speed = 0.25f;
    private Vector2 direction;
    private Rigidbody2D body;
    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        body.position += direction * speed;
    }

    public void SetDirection(Vector2 direction)
    {
        this.direction = direction;
    }
}
