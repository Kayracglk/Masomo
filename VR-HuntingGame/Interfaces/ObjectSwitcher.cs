using UnityEngine;

public class ObjectSwitcher : MonoBehaviour
{
    public GameObject[] objectList; // Unity Editor'da listeyi doldurun
    private int currentIndex = 0;

    void Start()
    {
        // Başlangıçta sadece ilk objeyi aktif et, diğerlerini deaktif et
        for (int i = 0; i < objectList.Length; i++)
        {
            if (i == currentIndex)
                objectList[i].SetActive(true);
            else
                objectList[i].SetActive(false);
        }
    }

    public void OnNextButtonClick()
    {
        // İleri butonuna tıklandığında bir sonraki objeyi aktif et
        DeactivateCurrentObject();
        currentIndex = (currentIndex + 1) % objectList.Length;
        ActivateCurrentObject();
    }

    public void OnBackButtonClick()
    {
        // Geri butonuna tıklandığında bir önceki objeyi aktif et
        DeactivateCurrentObject();
        currentIndex = (currentIndex - 1 + objectList.Length) % objectList.Length;
        ActivateCurrentObject();
    }

    private void ActivateCurrentObject()
    {
        // Şu anki indeksteki objeyi aktif et
        objectList[currentIndex].SetActive(true);
    }

    private void DeactivateCurrentObject()
    {
        // Şu anki indeksteki objeyi deaktif et
        objectList[currentIndex].SetActive(false);
    }
}
