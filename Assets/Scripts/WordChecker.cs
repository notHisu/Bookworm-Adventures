using System.Collections.Generic;
using UnityEngine;

public class WordChecker : MonoBehaviour
{
    private static WordChecker instance;
    private HashSet<string> validWords;

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

    void Awake()
    {
        if (instance != null && instance != this)
        {
            DestroyImmediate(gameObject); // Destroy duplicate if it exists
            return;
        }

        instance = this;
        LoadDictionary();
    }

    void LoadDictionary()
    {
        validWords = new HashSet<string>();
        var wordFile = Resources.Load<TextAsset>("Dictionaries/words_alpha");
        if (wordFile == null)
        {
            Debug.Log("Nothing");
        }
        else
        {
            Debug.Log("Dictionary Loaded.");
            string[] words = wordFile.text.Split('\n');
            foreach (string word in words)
            {
                validWords.Add(word.Trim().ToLower());
            }
        }
        // Debug.Log("Count: " + validWords.Count);
    }

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
