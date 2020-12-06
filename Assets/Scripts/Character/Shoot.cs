using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Projectile))]
public class Shoot : MonoBehaviour
{
    private Projectile projectile;

    // Start is called before the first frame update
    void Start()
    {
        projectile = GetComponent<Projectile>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
