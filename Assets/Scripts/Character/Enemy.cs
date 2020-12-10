using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Shoot))]
public class Enemy : MonoBehaviour
{
    private float shootDelay = 1f;
    private Shoot shoot;
    [SerializeField]
    private Transform projectileSpawner;

    // Start is called before the first frame update
    void Start()
    {
        shoot = GetComponent<Shoot>();
        InvokeRepeating("FireAtPlayer", shootDelay, shootDelay);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FireAtPlayer()
    {
        shoot.Fire(Vector2.down, projectileSpawner.position);
    }

}
