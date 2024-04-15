using System.Collections.Generic;
using UnityEngine;

// WordChecker is responsible for loading the dictionary from the resources folder and checking if a word is valid.
public class WordChecker : MonoBehaviour
{
    // Singleton pattern
    private static WordChecker instance;

    // HashSet is used to store the dictionary words
    private HashSet<string> validWords;

    // Get the instance of the WordChecker
    public static WordChecker Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<WordChecker>();
                if (instance == null)
                {
                    Debug.LogError("No WordChecker found in scene. Creating instance.");
                    instance = new WordChecker();
                }
            }
            return instance;
        }
    }

    // Awake is called when the script instance is being loaded
    void Awake()
    {
        // If there is already an instance of WordChecker and it is not this one, destroy this one
        if (instance != null && instance != this)
        {
            DestroyImmediate(gameObject); // Destroy duplicate if it exists
            return;
        }

        instance = this;
        LoadDictionary();
    }

    // Load the dictionary from the resources folder
    void LoadDictionary()
    {
        validWords = new HashSet<string>();
        var wordFile = Resources.Load<TextAsset>("Dictionaries/words_alpha");
        if (wordFile == null)
        {
            // Debug.Log("Nothing");
        }
        else
        {
            // Debug.Log("Dictionary Loaded.");

            // Split the text file by new line and add each word to the HashSet
            string[] words = wordFile.text.Split('\n');
            foreach (string word in words)
            {
                validWords.Add(word.Trim().ToLower());
            }
        }
        // Debug.Log("Count: " + validWords.Count);
    }

    // Check if a word is valid
    public bool IsValidWord(string word)
    {
        if (word.Length > 2)
        {
            return validWords.Contains(word.ToLower());
        }
        else
            return false;
    }
}
