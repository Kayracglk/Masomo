using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class ScorePanel : MonoBehaviour
{
    public TextMeshProUGUI playerNameText;
    public TextMeshProUGUI totalScoreText;
    public TextMeshProUGUI individualScoresText;

    public TextMeshProUGUI firstPlaceText;
    public TextMeshProUGUI secondPlaceText;
    public TextMeshProUGUI thirdPlaceText;

    void Start()
    {
        if (ScoreManager.Instance != null)
        {
            
            PlayerScore currentPlayerScore = ScoreManager.Instance.GetCurrentPlayerScore();

            
            if (currentPlayerScore != null)
            {
                playerNameText.text = "Oyuncu İsmi: " + currentPlayerScore.playerName;
                totalScoreText.text = "Toplam Puan: " + currentPlayerScore.totalScore;
                individualScoresText.text = FormatIndividualScores(currentPlayerScore.individualScores);
            }
            else
            {
                Debug.LogWarning("Current player score is null.");
            }

            
            ScoreManager.Instance.UpdateTopPlayersTexts(firstPlaceText, secondPlaceText, thirdPlaceText);
        }
        else
        {
            Debug.LogWarning("ScoreManager is not available.");
        }
        
        if (ScoreManager.Instance != null)
        {
           
            PlayerScore currentPlayerScore = ScoreManager.Instance.GetCurrentPlayerScore();

            
            if (currentPlayerScore != null)
            {
                playerNameText.text = "Oyuncu İsmi: " + currentPlayerScore.playerName;
                totalScoreText.text = "Toplam Puan: " + currentPlayerScore.totalScore;
                individualScoresText.text = FormatIndividualScores(currentPlayerScore.individualScores);
            }
            else
            {
                Debug.LogWarning("Current player score is null.");
            }
        }
        else
        {
            Debug.LogWarning("ScoreManager is not available.");
        }
    }

    string FormatIndividualScores(int[] scores) 
    {
        
        Dictionary<int, int> scoreCount = new Dictionary<int, int>();

        
        foreach (int score in scores)
        {
            if (scoreCount.ContainsKey(score))
            {
                scoreCount[score]++;
            }
            else
            {
                scoreCount[score] = 1;
            }
        }

        
        List<string> scoreDetails = new List<string>();

        if (scoreCount.ContainsKey(10))
        {
            int count = scoreCount[10];
            scoreDetails.Add($"{count}x Kafadan vurdun");
        }

        if (scoreCount.ContainsKey(2))
        {
            int count = scoreCount[2];
            scoreDetails.Add($"{count}x Kalçadan vurdun");
        }

        if (scoreCount.ContainsKey(5))
        {
            int count = scoreCount[5];
            scoreDetails.Add($"{count}x Gövdeden vurdun");
        }

        
        return string.Join(", ", scoreDetails);
    }
}
