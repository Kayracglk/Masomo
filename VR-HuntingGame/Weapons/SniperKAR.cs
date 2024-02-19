using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperKAR : MonoBehaviour
{
    public WeaponWheelSelect weaponWheelSelect;

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "rightHand")
        {
            weaponWheelSelect.SelectWeapon("Kar-98");
        }
    }
}
