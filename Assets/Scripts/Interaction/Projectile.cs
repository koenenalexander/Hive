﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    [SerializeField]
    private float speed = 2f;
    [SerializeField]
    private float fireDelay = .5f;
    private Vector2 direction;
    private Rigidbody2D body;
    private float timeToExpire = 1f;
    public float FireDelay => fireDelay;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        Destroy(gameObject, timeToExpire);
    }

    void FixedUpdate()
    {
        body.velocity = direction * speed;
    }

    public void SetDirection(Vector2 direction)
    {
        this.direction = direction;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var handler = collision.gameObject.GetComponent<DamageHandler>();
        if (handler != null)
        {
            handler.Damage(1);
        }
        Destroy(gameObject, .2f);
    }
}
