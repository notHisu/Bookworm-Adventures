using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TURNS {Start, PlayerTurn, Processing, EnemyTurn, Victory, Lost }

public class BattleSystem : MonoBehaviour
{
    private static BattleSystem instance;

    private Dictionary<int, double> damageValues = new Dictionary<int, double>()
    {
        {1, 0}, {2, 0.25}, {3, 0.5}, {4, 0.75}, {5, 1},
        {6, 1.5}, {7, 2}, {8, 2.75}, {9, 3.5}, {10, 4.5}, {11, 5.5},
        {12, 6.75}, {13, 8}, {14, 9.5}, {15, 11}, {16, 13}
    };

    [SerializeField]
    private TURNS turn;

    [SerializeField]
    private BattleUIManager battleUIManager;

    private GameObject player;
    private GameObject enemy;

    public static BattleSystem Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<BattleSystem>();
                if (instance == null)
                {
                    Debug.LogError("No BattleSystem found in scene. Creating instance.");
                    instance = new BattleSystem();
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
        DontDestroyOnLoad(gameObject); // Persist across scenes (optional)
    }

    private void Start()
    {
         turn = TURNS.Start;
        battleUIManager.UpdateTurnIndicator(turn.ToString());
        battleUIManager.SetUpCharacterInfo();
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
        if (turn == TURNS.PlayerTurn)
        {
            Debug.Log("Word damage: " + GetWordDamage());
            LetterGrid.Instance.ResetSelectedTiles();
        }
        else return;
    }

    public void OnScrambleButton()
    {
        if (turn == TURNS.PlayerTurn)
        {
            Debug.Log("Scrambleee!");
            LetterGrid.Instance.ScrambleLetter();
        }
        else return;
    }

}
