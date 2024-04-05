using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum TURNS
{
    Start,
    PlayerTurn,
    Processing,
    EnemyTurn,
    Victory,
    Defeated
}

public class BattleSystem : MonoBehaviour
{
    private static BattleSystem instance;

    private Dictionary<int, double> damageValues = new Dictionary<int, double>()
    {
        { 1, 0 },
        { 2, 0.25 },
        { 3, 0.5 },
        { 4, 0.75 },
        { 5, 1 },
        { 6, 1.5 },
        { 7, 2 },
        { 8, 2.75 },
        { 9, 3.5 },
        { 10, 4.5 },
        { 11, 5.5 },
        { 12, 6.75 },
        { 13, 8 },
        { 14, 9.5 },
        { 15, 11 },
        { 16, 13 }
    };

    [SerializeField]
    private TURNS turn;

    [SerializeField]
    private BattleUIManager battleUIManager;

    private EnemySpawner enemySpawner;
    private GameObject playerObject;
    private GameObject enemyObject;

    private Player player;
    private Enemy enemy;
    public int TotalScore = 0;

    private Coroutine currentCoroutine;

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
    }

    private void Start()
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        currentCoroutine = StartCoroutine(Setup());
    }

    IEnumerator Setup()
    {
        SetState(TURNS.Start);
        battleUIManager.SetUpCharacterInfo();
        battleUIManager.DisableButtons();

        enemySpawner = GameObject.Find("EnemySpawner").GetComponent<EnemySpawner>();
        playerObject = GameObject.FindWithTag("Player");
        enemyObject = GameObject.FindWithTag("Enemy");

        if (playerObject != null)
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

        yield return new WaitForSeconds(1f);
        PlayerTurn();
    }

    void SetState(TURNS nextTurn)
    {
        turn = nextTurn;
        battleUIManager.UpdateTurnIndicator(nextTurn.ToString());
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
        if (GetWordDamage() > 0)
        {
            if (turn == TURNS.PlayerTurn)
            {
                Debug.Log("Word damage: " + GetWordDamage());
                if (currentCoroutine != null)
                {
                    StopCoroutine(currentCoroutine);
                }
                currentCoroutine = StartCoroutine(PlayerAttack());
            }
        }
    }

    IEnumerator PlayerAttack()
    {
        yield return new WaitForSeconds(.2f);

        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }

        enemy.TakeDamage(GetWordDamage() + player.SendDamage());
        battleUIManager.UpdateCharacterHUD();

        LetterGrid.Instance.ResetSelectedTiles();

        battleUIManager.DisableButtons();

        if (enemy.GetHealth() > 0)
        {
            currentCoroutine = StartCoroutine(EnemyTurn());
        }
        else
        {
            enemy.Die(1);
            currentCoroutine = StartCoroutine(SetupNewEnemy());
        }
    }

    IEnumerator SetupNewEnemy()
    {
        yield return new WaitForSeconds(1f);

        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }

        enemySpawner.CreateEnemy();

        enemyObject = GameObject.FindWithTag("Enemy");

        if (enemyObject != null)
        {
            battleUIManager.SetUpCharacterInfo();
            enemy = enemyObject.GetComponent<Enemy>();

            Debug.Log("New enemy spawned!!");
            PlayerTurn();
        }
        else
        {
            currentCoroutine = StartCoroutine(EndGame());
        }
    }

    public void OnScrambleButton()
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        if (turn == TURNS.PlayerTurn)
        {
            Debug.Log("Scrambleee!");
            LetterGrid.Instance.ScrambleLetter();
            battleUIManager.DisableButtons();
            currentCoroutine = StartCoroutine(EnemyTurn());
        }
        else
            return;
    }

    void PlayerTurn()
    {
        SetState(TURNS.PlayerTurn);

        if (turn == TURNS.PlayerTurn)
        {
            battleUIManager.EnableButtons();
        }
    }

    IEnumerator EnemyTurn()
    {
        SetState(TURNS.EnemyTurn);

        if (turn == TURNS.EnemyTurn)
        {
            yield return new WaitForSeconds(1.5f);

            player.TakeDamage(enemy.SendDamage());

            battleUIManager.UpdateCharacterHUD();

            yield return new WaitForSeconds(1f);
            if (player.GetHealth() > 0)
            {
                PlayerTurn();
            }
            else
            {
                player.Die();
                currentCoroutine = StartCoroutine(EndGame());
            }
        }
    }

    IEnumerator EndGame()
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }

        yield return new WaitForSeconds(1f);
        SetState(TURNS.Victory);
        SceneManager.LoadScene("Victory");
    }
}
