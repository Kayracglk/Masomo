using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperSWD : MonoBehaviour
{
    public WeaponWheelSelect weaponWheelSelect;

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "rightHand")
        {
            weaponWheelSelect.SelectWeapon("SVD");
        }
    }
}
