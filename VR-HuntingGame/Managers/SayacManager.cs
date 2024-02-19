using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SayacManager : MonoBehaviour
{
    public static SayacManager Instance { get; private set; }

    
    public int sayaçSüresi;

    private float geçenSüre;
    private bool sayaçBaşladı = false;

    private void Start()
    {
        Baslat(sayaçSüresi);
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (sayaçBaşladı)
        {
            geçenSüre += Time.deltaTime;

            if (geçenSüre >= sayaçSüresi)
            {
                SayaçBitti();
            }
        }
    }

    public void Baslat(int dakika)
    {
        if (sayaçBaşladı)
        {
            Debug.LogWarning("Sayaç zaten başlatıldı.");
            return;
        }

        sayaçSüresi = dakika * 60; 

        geçenSüre = 0f;
        sayaçBaşladı = true;
    }

    private void SayaçBitti()
    {
        sayaçBaşladı = false;
        geçenSüre = 0f;

        
        string hedefSahneAdı = "Score";
        SceneManager.LoadScene(hedefSahneAdı);
    }
}
