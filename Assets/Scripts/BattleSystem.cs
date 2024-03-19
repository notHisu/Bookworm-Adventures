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

    private GameObject playerObject;
    private GameObject enemyObject;

    private Player player;
    private Enemy enemy;

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

        playerObject = GameObject.FindWithTag("Player");
        enemyObject = GameObject.FindWithTag("Enemy");

        if( playerObject != null)
        {
            player = playerObject.GetComponent<Player>();
        }
        else
        {
            Debug.Log("Can't find player in the current scene");
        }

        if (enemyObject != null)
        {
            enemy = enemyObject.GetComponent<Enemy>();

        }
        else
        {
            Debug.Log("No enemy in the current scene");
        }

        turn = TURNS.PlayerTurn;
        battleUIManager.UpdateTurnIndicator(turn.ToString());

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

            enemy.TakeDamage(GetWordDamage());

            battleUIManager.UpdateCharacterHUD();

            turn = TURNS.EnemyTurn;
            battleUIManager.UpdateTurnIndicator(turn.ToString());
            StartCoroutine(EnemyTurn());

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

    IEnumerator EnemyTurn()
    {
        player.TakeDamage(enemy.SendDamage());

        battleUIManager.UpdateCharacterHUD();

        yield return new WaitForSeconds(1f);
    }

}
