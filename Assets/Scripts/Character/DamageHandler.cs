using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageHandler : MonoBehaviour
{
    private List<ProcessDamage> handlers = new List<ProcessDamage>();
    public delegate void ProcessDamage(int damage);

    public void AddHandler(ProcessDamage handler)
    {
        handlers.Add(handler);
    }

    public void Damage(int damage)
    {
        foreach (var handler in handlers)
            handler(damage);
    }
}
