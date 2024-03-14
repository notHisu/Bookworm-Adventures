using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum TURNS { PlayerTurn, EnemyTurn }

public class BattleSystem : MonoBehaviour
{
    private static BattleSystem _instance;

    private Dictionary<int, double> damageValues = new Dictionary<int, double>()
    {
        {1, 0}, {2, 0.25}, {3, 0.5}, {4, 0.75}, {5, 1},
        {6, 1.5}, {7, 2}, {8, 2.75}, {9, 3.5}, {10, 4.5}, {11, 5.5},
        {12, 6.75}, {13, 8}, {14, 9.5}, {15, 11}, {16, 13}
    };

    public static BattleSystem Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<BattleSystem>();
                if (_instance == null)
                {
                    Debug.LogError("No BattleSystem found in scene. Creating instance.");
                    _instance = new BattleSystem();
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
    }

    double GetWordDamage()
    {
        int wordValue = LetterGrid.Instance.GetSelectedWordValue();

        if (damageValues.TryGetValue(wordValue, out double value))
        {
            return value;
        }
        else
        {
            return 0;
        }
    }

    public void OnAttackButton()
    {
        Debug.Log("Is valid: " + WordChecker.Instance.isValid);
        if (WordChecker.Instance.isValid)
        {
            Debug.Log("Word damage: " + GetWordDamage());
            // LetterGrid.Instance.ResetSelectedTiles();
        }
    }

    public void OnScrambleButton()
    {
        LetterGrid.Instance.ScrambleLetter();
    }

}
