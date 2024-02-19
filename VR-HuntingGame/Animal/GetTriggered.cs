using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetTriggered : MonoBehaviour, IDamagable
{
    BirdMovement bird;

    private void Awake()
    {
        bird = GetComponent<BirdMovement>();
    }

    public void GiveDamage(Vector3 hitPosition, float damage)
    {
        bird.canFly = true;
    }
}
