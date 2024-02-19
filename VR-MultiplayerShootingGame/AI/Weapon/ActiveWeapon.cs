using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveWeapon : MonoBehaviour
{
    public Transform crosshairTarget;
    private AIRaycastWeapon weapon;

    private void Awake()
    {
        AIRaycastWeapon existingWeapon = GetComponentInChildren<AIRaycastWeapon>();

        if(existingWeapon != null )
        {
            Equip(existingWeapon);
        }
    }

    private void Update()
    {
        if (weapon != null)
        {
            
        }
    }

    public void Equip(AIRaycastWeapon newWeapon)
    {
        weapon = newWeapon;
    }

}
