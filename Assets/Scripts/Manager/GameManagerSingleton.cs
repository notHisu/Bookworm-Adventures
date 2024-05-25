using Unity.VisualScripting;
using UnityEngine;
/// <summary>
/// 
/// </summary>
public sealed class GameManagerSingleton
{
    // Singleton instance of the GameManager
    private GameManagerSingleton()
    {

    }

    public static GameManagerSingleton Instance { get { return Nested.instance; } }

    private class Nested
    {
        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static Nested()
        {
        }

        internal static readonly GameManagerSingleton instance = new();
    }

    // Current score in the game
    private double currentScore = 0;

    // Best score achieved in the game
    private double bestScore;

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
    public void LoadBestScore()
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

    public void ResetScore()
    {
        currentScore = 0;
    }
}
