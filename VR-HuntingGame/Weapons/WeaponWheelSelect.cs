using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;

public class WeaponWheelSelect : MonoBehaviour
{
    [Tooltip("Bu öğeyi sağ veya sol elin Yakalayıcısı olarak kullanmak istediğiniz el yakalayıcısını belirtin.")]
    public Grabber handGrabber;

    [System.Serializable]
    public class WeaponInfo
    {   
        public string weaponName;
        public Grabbable equipGrabbable;
        public GameObject weaponObject;
        public GameObject cubeOrigin;
    }
    
    public List<WeaponInfo> weapons = new List<WeaponInfo>();

    public void SelectWeapon(string weaponName)
    {
        WeaponInfo selectedWeapon = null;

        foreach (var weapon in weapons)
        {
            if (weapon.weaponName == weaponName)
            {
                selectedWeapon = weapon;
                break;
            }
        }
        
        if (selectedWeapon == null)
        {
            Debug.LogError("Silah bulunamadı: " + weaponName);
            return;
        }

        // Seçilen silah zaten eldeyse bir şey yapma
        if (handGrabber.HeldGrabbable == selectedWeapon.equipGrabbable)
        {
            Debug.LogWarning("Seçilen silah zaten elde: " + weaponName);
            return;
        }

        // Elinde bir şey varsa, bırak
        if (handGrabber.HeldGrabbable != null)
        {
            foreach (var weapon in weapons)
            {
                if (weapon.equipGrabbable == handGrabber.HeldGrabbable)
                {
                    handGrabber.HeldGrabbable.DropItem(handGrabber);
                    weapon.equipGrabbable.transform.position = weapon.cubeOrigin.transform.position;
                    weapon.equipGrabbable.transform.rotation = weapon.cubeOrigin.transform.rotation;
                    weapon.equipGrabbable.GetComponent<Rigidbody>().isKinematic = true;
                    weapon.weaponObject.SetActive(false);
                    Debug.Log("Tutulan silah bırakıldı: " + weapon.weaponName);
                    break;
                }
            }
        }

        // Seçilen silahı etkinleştirin, böylece görünür hale gelir
        selectedWeapon.weaponObject.SetActive(true);

        // Seçilen silahı takın
        handGrabber.GrabGrabbable(selectedWeapon.equipGrabbable);

        // Kinematik durumunu false olarak ayarlayın
        selectedWeapon.equipGrabbable.GetComponent<Rigidbody>().isKinematic = false;

        // Ebeveynini null olarak ayarlayın, böylece silah tekerleğiyle kaybolmaz
        selectedWeapon.equipGrabbable.transform.SetParent(null);
    }

    public void EmptyHandSelect()
    {
        // Elin zaten boşsa bir şey yapma
        if (handGrabber.HeldGrabbable == null)
        {
            return;
        }
        
        foreach (var weapon in weapons)
        {
            // Tutulan silahlardan biri ise bırakın ve başlangıç konumuna geri gönderin
        if (handGrabber.HeldGrabbable == weapon.equipGrabbable)
            {
                Debug.Log("Silah bırakılmaya çalışıldı.");
                
                // Diğer Debug.Log() ifadeleri ekle ve nesneleri kontrol et
                Debug.Log("weapon.equipGrabbable: " + weapon.equipGrabbable);
                Debug.Log("handGrabber.HeldGrabbable: " + handGrabber.HeldGrabbable);
                
                handGrabber.HeldGrabbable.DropItem(handGrabber);
                weapon.equipGrabbable.transform.position = weapon.cubeOrigin.transform.position;
                weapon.equipGrabbable.transform.rotation = weapon.cubeOrigin.transform.rotation;
                weapon.equipGrabbable.GetComponent<Rigidbody>().isKinematic = true;
                weapon.weaponObject.SetActive(false);
                Debug.Log("Tutulan silah bırakıldı: " + weapon.weaponName);
                break;
            }

        }
    }
}

