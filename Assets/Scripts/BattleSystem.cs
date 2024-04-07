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

    private Coroutine currentCoroutine;

    private float playerTurnDelay = 1f;
    private float enemyTurnDelay = 2f;
    private float processingDelay = 1f;

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

        yield return new WaitForSeconds(processingDelay);
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
                ScoreManager.Instance.AddScore(GetWordDamage());
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
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }

        enemy.TakeDamage(GetWordDamage() + player.SendDamage());

        yield return new WaitForSeconds(playerTurnDelay);

        battleUIManager.UpdateCharacterHUD();

        yield return new WaitForSeconds(processingDelay);

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

        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }

        yield return new WaitForSeconds(processingDelay);

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

            player.TakeDamage(enemy.SendDamage());

            yield return new WaitForSeconds(enemyTurnDelay);

            battleUIManager.UpdateCharacterHUD();

            yield return new WaitForSeconds(processingDelay);

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

        yield return new WaitForSeconds(processingDelay);
        SetState(TURNS.Victory);
        SceneManager.LoadScene("Victory");
    }
}
