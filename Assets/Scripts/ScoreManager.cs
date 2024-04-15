using TMPro;
using UnityEngine;

// Class to manage the game's score
public class ScoreManager : MonoBehaviour
{
    // Singleton instance of the ScoreManager
    private static ScoreManager instance;

    // Current score in the game
    private double currentScore;

    // Best score achieved in the game
    private double bestScore;

    // Property to get the singleton instance of the ScoreManager
    public static ScoreManager Instance
    {
        get
        {
            // If the instance is null, find the ScoreManager in the scene or create a new one
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

    // Method called when the script instance is being loaded
    void Awake()
    {
        // If the instance is not null and not this, destroy this duplicate
        if (instance != null && instance != this)
        {
            DestroyImmediate(gameObject);
            return;
        }

        // Don't destroy this object when loading a new scene
        DontDestroyOnLoad(gameObject);
        // Set the instance to this
        instance = this;
        // Load the best score
        LoadBestScore();
    }

    // Method to add to the current score
    public void AddScore(double scoreToAdd)
    {
        // Add the score to the current score
        currentScore += scoreToAdd;
        // If the current score is greater than the best score, update the best score and save it
        if (currentScore > bestScore)
        {
            bestScore = currentScore;
            SaveBestScore();
        }
    }

    // Method to get the current score
    public double GetCurrentScore()
    {
        return currentScore;
    }

    // Method to get the best score
    public double GetBestScore()
    {
        return bestScore;
    }

    // Method to load the best score
    private void LoadBestScore()
    {
        // Load the best score from PlayerPrefs
        bestScore = PlayerPrefs.GetFloat("BestScore", 0);
    }

    // Method to save the best score
    private void SaveBestScore()
    {
        // Save the best score to PlayerPrefs
        PlayerPrefs.SetFloat("BestScore", (float)bestScore);
    }
}
