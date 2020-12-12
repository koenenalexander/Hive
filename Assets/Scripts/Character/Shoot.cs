using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public void Fire(Vector2 direction, Vector3 spawnPoint, GameObject projectile)
    {
        var shot = Instantiate(projectile, spawnPoint, transform.rotation);
        var projectileComponent = shot.GetComponent<Projectile>();
        projectileComponent.SetDirection(direction);
    }
}
