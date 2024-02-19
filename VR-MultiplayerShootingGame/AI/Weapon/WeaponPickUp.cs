using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickUp : MonoBehaviour
{
    public AIRaycastWeapon weaponPrefab;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            AIWeapons weapons = other.gameObject.GetComponent<AIWeapons>();
            if (weapons != null)
            {
                AIRaycastWeapon newWeapon = Instantiate(weaponPrefab);
                weapons.Equip(newWeapon);
                gameObject.SetActive(false);
            }
        }
    }
}
