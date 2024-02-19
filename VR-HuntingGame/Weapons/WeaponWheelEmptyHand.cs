using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponWheelEmptyHand : MonoBehaviour
{
    public WeaponWheelSelect weaponWheelSelect;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "rightHand")
        {
            weaponWheelSelect.EmptyHandSelect();
        }
    }
}
