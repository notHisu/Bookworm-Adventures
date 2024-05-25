// This code is SOLID, no need changes.
using System.Collections.Generic;
using UnityEngine;

// WordChecker is responsible for loading the dictionary from the resources folder and checking if a word is valid.
public class WordChecker : MonoBehaviour
{
    // HashSet is used to store the dictionary words
    private HashSet<string> validWords;

    // Awake is called when the script instance is being loaded
    void Awake()
    {
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
