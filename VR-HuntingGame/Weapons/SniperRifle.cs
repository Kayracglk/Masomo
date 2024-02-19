using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperRifle : MonoBehaviour
{
    public WeaponWheelSelect weaponWheelSelect;

    public void OnTriggerEnter(Collider other)
    {
        
        if (other.tag == "rightHand")
        {
            weaponWheelSelect.SelectWeapon("Sniper-Rifle");
        }
    }
}
