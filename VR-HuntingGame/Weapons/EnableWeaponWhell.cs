using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableWeaponWhell: MonoBehaviour
{
    public GameObject weaponWheel; // Silah tekerleği nesnesi
    public GameObject targetposition; // Hedef pozisyon nesnesi
    public GameObject[] objectsToToggle; // Etkinleştirilip devre dışı bırakılacak objeler
    private int currentObjectIndex = 0; // Şu anki obje indeksi
    private bool thumbstickPressed = false; // Thumbstick basılı mı?

    void Update()
    {
        // "Oculus_CrossPlatform_SecondaryThumbstick" tuşuna basıldığında ve daha önce basılmamışsa
        if (Input.GetButton("Oculus_CrossPlatform_SecondaryThumbstick") && !thumbstickPressed)
        {
            thumbstickPressed = true; // Thumbstick basıldı olarak işaretle
            
            // Silah tekerleğini etkinleştir ve hedef pozisyona taşı
            if (!weaponWheel.activeSelf)
            {
                weaponWheel.SetActive(true);
                weaponWheel.transform.position = targetposition.transform.position;
                // weaponWheel.transform.rotation = targetposition.transform.rotation;
            }

            // Mevcut indeksteki objeyi devre dışı bırak
            if (objectsToToggle[currentObjectIndex].activeSelf)
            {
                objectsToToggle[currentObjectIndex].SetActive(false);
            }

            // Bir sonraki indekse geç
            currentObjectIndex = (currentObjectIndex + 1) % objectsToToggle.Length;

            // Yeni indeksteki objeyi etkinleştir
            objectsToToggle[currentObjectIndex].SetActive(true);
        }
        // "Oculus_CrossPlatform_SecondaryThumbstick" tuşunu bıraktığınızda
        else if (!Input.GetButton("Oculus_CrossPlatform_SecondaryThumbstick"))
        {
            thumbstickPressed = false; // Thumbstick bırakıldı olarak işaretle
            weaponWheel.SetActive(false); // Silah tekerleğini kapat
        }
    }
}
