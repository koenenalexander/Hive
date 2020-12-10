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
            Debug.LogError("Shoot.cs - Start() - Shoot must have a projectile assgined in the editor", projectile);
    }

    public void Fire(Vector2 direction, Vector3 spawnPoint)
    {
        var shot = Instantiate(projectile, spawnPoint, transform.rotation);
        var projectileComponent = shot.GetComponent<Projectile>();
        projectileComponent.SetDirection(direction);
    }
}
