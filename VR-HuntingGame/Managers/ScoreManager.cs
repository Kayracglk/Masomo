using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using TMPro;

[System.Serializable]
public class PlayerScore
{
    public string playerName = ""; 
    public int totalScore = 0; 
    public int[] individualScores = new int[0]; 
}

[System.Serializable]
public class PlayerScores
{
    public List<PlayerScore> PlayerScoreses = new List<PlayerScore>(); // Play score nesnesini listeye doldurdurma
}

public class ScoreManager : MonoBehaviour
{
    private static PlayerScore currentPlayerScore; 
    PlayerScores playerScores = new PlayerScores(); 
    private static ScoreManager instance; 

    private string dataFilePath; 

    public void Start()
    {
        LoadPlayerData(); 
    }

    public static ScoreManager Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.LogError("ScoreManager instance is null. Make sure to create an instance before accessing it.");
            }
            return instance; 
        }
    }

    private void Awake()
    {
        instance = this; 
        DontDestroyOnLoad(gameObject); // Sahneler arasında yok etmeyi engelle

        //dataFilePath = Application.dataPath + "/Resources/playerdata.json"; // Şuanki kaydedilen verilerin yolu (Değiştirebilinir)
        dataFilePath = Path.Combine(Application.persistentDataPath, "playerdata.json");


        if (currentPlayerScore == null)
        {
            currentPlayerScore = new PlayerScore(); // Şu anki oyuncu skoru nesnesini oluşturur
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
        LoadPlayerData(); // Oyuncu verilerini yükleme işlemini başlatan metot
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Score")
        {
            // Eğer sahne Score olursa verileri kaydet
            SavePlayerData();
            Debug.Log(scene.name);
        }
    }

    public PlayerScore GetCurrentPlayerScore()
    {
        return currentPlayerScore; // Şu anki oyuncu skorunu döndüren metot
    }

    public void SetCurrentPlayer(string playerName)
    {
        if (!string.IsNullOrEmpty(playerName))
        {
            currentPlayerScore.playerName = playerName; // Şu anki oyuncu adını ayarlayan metot
        }
        else
        {
            Debug.LogWarning("Uyarı: Oyuncu adı boş olamaz");
        }
    }

    public void AddScore(int score)
    {
        if (currentPlayerScore != null)
        {
            currentPlayerScore.totalScore += score; 

            int[] newScores = new int[currentPlayerScore.individualScores.Length + 1];
            currentPlayerScore.individualScores.CopyTo(newScores, 0);
            newScores[currentPlayerScore.individualScores.Length] = score;

            currentPlayerScore.individualScores = newScores;

            Debug.Log($"{currentPlayerScore.playerName}: Total Score = {currentPlayerScore.totalScore}, Individual Scores = {string.Join(",", currentPlayerScore.individualScores)}");
        }
        else
        {
            Debug.LogWarning("Şu anki oyuncu ayarlanmamış. Skor eklemeden önce oyuncuyu ayarlayın.");
        }
    }

    public void LoadPlayerData()
    {
        if (!File.Exists(dataFilePath))
        {
            // Dosya mevcut değilse yeni boş bir PlayerScores nesnesi oluşturur
            playerScores = new PlayerScores();
        }
        else
        {
            
            string jsonData = File.ReadAllText(dataFilePath);
            playerScores = JsonUtility.FromJson<PlayerScores>(jsonData);
        }
    }

    public void SavePlayerData()
    {
        if (playerScores == null)
        {
            playerScores = new PlayerScores();
        }

        playerScores.PlayerScoreses.Add(currentPlayerScore); // Şu anki oyuncu skorunu PlayerScores listesine ekler
        string json = JsonUtility.ToJson(playerScores, true); // PlayerScores nesnesini JSON formatına çevirir

        File.WriteAllText(dataFilePath, json); // JSON verisini dosyaya yazar
    }

    // Tüm kaydedilen oyuncu verilerini gösterr
    public void ShowAllPlayerData()
    {
        if (File.Exists(dataFilePath))
        {
            string jsonData = File.ReadAllText(dataFilePath);
            PlayerScore[] allPlayers = JsonUtility.FromJson<PlayerScore[]>(jsonData);

            foreach (var player in allPlayers)
            {
                Debug.Log($"Player Data: Player Name = {player.playerName}, Total Score = {player.totalScore}, Individual Scores = {string.Join(",", player.individualScores)}");
            }
        }
    }
    public void UpdateTopPlayersTexts(TextMeshProUGUI firstPlaceText, TextMeshProUGUI secondPlaceText, TextMeshProUGUI thirdPlaceText)
    {
        if (File.Exists(dataFilePath))
        {
            string jsonData = File.ReadAllText(dataFilePath);
            playerScores = JsonUtility.FromJson<PlayerScores>(jsonData);

            if (playerScores != null && playerScores.PlayerScoreses.Count > 0)
            {
                // Sıralanmış oyuncu skorlarını al (yüksek puandan düşüğe)
                List<PlayerScore> sortedScores = playerScores.PlayerScoreses.OrderByDescending(p => p.totalScore).ToList();

                // scorları sıralı şekilde yazmayı sağlar
                if (sortedScores.Count >= 1)
                {
                    firstPlaceText.text = $"{sortedScores[0].playerName}: {sortedScores[0].totalScore} puan";
                }

                if (sortedScores.Count >= 2)
                {
                    secondPlaceText.text = $"{sortedScores[1].playerName}: {sortedScores[1].totalScore} puan";
                }

                if (sortedScores.Count >= 3)
                {
                    thirdPlaceText.text = $"{sortedScores[2].playerName}: {sortedScores[2].totalScore} puan";
                }
            }
            else
            {
                Debug.LogWarning("Player scores are not available.");
            }
        }
        else
        {
            Debug.LogWarning($"Data file not found at path: {dataFilePath}");
        }
    }

    
}
