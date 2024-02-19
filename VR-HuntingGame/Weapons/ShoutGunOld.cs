using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoutGunOld : MonoBehaviour
{
    // Start is called before the first frame update    public WeaponWheelSelect weaponWheelSelect;
    public WeaponWheelSelect weaponWheelSelect;

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "rightHand")
        {
            weaponWheelSelect.SelectWeapon("ShotgunNormal");
        }
    }
}
