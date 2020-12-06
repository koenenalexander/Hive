using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField]
    private GameObject projectile;

    // Start is called before the first frame update
    void Start()
    {
        if (projectile == null)
            Debug.LogError("Shoot.cs - Start() - No projectile assigned!", projectile);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
