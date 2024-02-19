using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviourPunCallbacks
{
    [SerializeField] private HealthManager health;

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            GiveDamage(20);
        }
    }

    public void GiveDamage(float damage)
    {
        health.TakeDamage(damage);
    }
}
