using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;

public class EneableMap : MonoBehaviour
{
    public GameObject HaritaAktif;

    private bool isYButtonPressed = false; 

    private void Start(){

        HaritaAktif.gameObject.SetActive(false);
    }

    private void Update()
    {
        // Y düğmesine basıldığında kontrol et
        if (InputBridge.Instance.YButton && !isYButtonPressed)
        {
            isYButtonPressed = true; 
            ObjectToggle();
        }
        else if (!InputBridge.Instance.YButton && isYButtonPressed)
        {
            isYButtonPressed = false; 
        }
    }

    private void ObjectToggle()
    {
        // Objeyi aktifse deaktif deaktifse aktif yap
        HaritaAktif.SetActive(!HaritaAktif.activeSelf);
    }
}
