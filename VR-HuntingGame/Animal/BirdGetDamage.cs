using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdGetDamage : MonoBehaviour, IDamagable
{
    private BirdHealthManager bird;

    private void Awake()
    {
        bird = GetComponent<BirdHealthManager>();
    }

    public void GiveDamage(Vector3 hitPosition = default, float damage = 1)
    {
        bird.GiveDamage();
    }
}
