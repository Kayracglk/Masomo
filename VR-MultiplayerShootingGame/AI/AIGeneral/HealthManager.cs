using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public float MaxHealth;
    private float CurrentHealth;

    private void Awake()
    {
        CurrentHealth = MaxHealth;
        OnAwake();
    }

    private void Start()
    {
        OnStart();
    }

    public void TakeDamage(float amounth)
    {
        CurrentHealth -= amounth;
        OnDamaged();
        if(CurrentHealth <= 0)
        {
            Die();
        }
    }

    public bool IsDead()
    {
        return CurrentHealth <= 0;
    }

    private void Die()
    {
        OnDeath();
    }

    protected virtual void OnStart() { }
    protected virtual void OnAwake() { }
    protected virtual void OnDamaged() { }
    protected virtual void OnDeath() { }
}
