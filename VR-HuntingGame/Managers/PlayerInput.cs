using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class PlayerInput : MonoBehaviour
{
    public TMP_InputField playerNameInput;
    private ScoreManager scoreManager; 

    void Start()
    {
        
        if (scoreManager == null)
        {
            scoreManager = FindObjectOfType<ScoreManager>(); 
        }

        if (scoreManager == null)
        {
            Debug.LogWarning("ScoreManager, PlayerInput'a atanmamış ve sahnede ScoreManager örneği bulunamadı. Unity Editor'da atadığınızdan veya bir örnek oluşturduğunuzdan emin olun.");
        }
    }

    public void SaveName()
    {
        string playerName;

        if (string.IsNullOrEmpty(playerNameInput.text))
        {
            playerName = "Player" + RandomNumber(); // rastgele isim ver
        }
        else
        {
            playerName = playerNameInput.text; 
        }

        if (scoreManager != null)
        {
            scoreManager.SetCurrentPlayer(playerName); // ScoreManager şu anki oyuncu adını ayarlayan metot
            Debug.Log("İsim kaydedildi: " + playerName); 

            
            StartCoroutine(LoadSceneDelayed("Hunting", 1f));
        }
        else
        {
            Debug.LogWarning("ScoreManager is not assigned to PlayerInput, and no ScoreManager instance was found in the scene. Make sure to assign it in the Unity Editor or create an instance.");

        }
    }

    private int RandomNumber()
    {
        // Rastgele 4 haneli dsyı üret
        return Random.Range(1000, 10000);
    }

    IEnumerator LoadSceneDelayed(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName); 
    }
}
