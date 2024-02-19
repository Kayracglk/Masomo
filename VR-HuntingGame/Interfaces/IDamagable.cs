using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    public void GiveDamage(Vector3 hitPosition = new Vector3(), float damage = 1f);
}
