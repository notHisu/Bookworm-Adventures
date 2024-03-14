using System.Collections.Generic;
using UnityEngine;

public class WordChecker : MonoBehaviour
{
    private static WordChecker _instance;
    private HashSet<string> validWords;

    public bool isValid = false;

    public static WordChecker Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<WordChecker>();
                if (_instance == null)
                {
                    Debug.LogError("No WordChecker found in scene. Creating instance.");
                    _instance = new WordChecker();
                }
            }
            return _instance;
        }
    }

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            DestroyImmediate(gameObject); // Destroy duplicate if it exists
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject); // Persist across scenes (optional)
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
            // Debug.Log("Something");
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
        else return false;
    }

    private void SetIsValid()
    {
        isValid = !isValid;
    }
}
