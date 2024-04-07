using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    private static ScoreManager instance;

    private double currentScore;
    private double bestScore;



    public static ScoreManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ScoreManager>();
                if (instance == null)
                {
                    Debug.LogError("No ScoreManager found in scene. Creating instance.");
                    instance = new ScoreManager();
                }
            }
            return instance;
        }
    }

    void Awake()
    {
        if (instance != null && instance != this)
        {
            DestroyImmediate(gameObject); // Destroy duplicate if it exists
            return;
        }

        DontDestroyOnLoad(gameObject);
        instance = this;
        LoadBestScore();
    }

    public void AddScore(double scoreToAdd)
    {
        currentScore += scoreToAdd;
        if (currentScore > bestScore)
        {
            bestScore = currentScore;
            SaveBestScore();
        }
    }

    public double GetCurrentScore()
    {
        return currentScore;
    }

    public double GetBestScore()
    {
        return bestScore;
    }

    private void LoadBestScore()
    {
        bestScore = PlayerPrefs.GetFloat("BestScore", 0);
    }

    private void SaveBestScore()
    {
        PlayerPrefs.SetFloat("BestScore", (float)bestScore);
    }
}
