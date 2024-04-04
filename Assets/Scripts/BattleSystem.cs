using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEditor.Experimental.GraphView.GraphView;

public enum TURNS { Start, PlayerTurn, Processing, EnemyTurn, Victory, Defeated }

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

    private EnemySpawner enemySpawner;
    private GameObject playerObject;
    private GameObject enemyObject;

    private Player player;
    private Animator playerAnimator;
    private Animator enemyAnimator;
    private Enemy enemy;
    public AudioSource playerAttck;
    public AudioSource enemyAttck;
    public List<string> usedWords;

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
        StartCoroutine(Setup());
    }

    IEnumerator Setup()
    {
        UpdateTurnIndicator(TURNS.Start);
        battleUIManager.SetUpCharacterInfo();
        battleUIManager.DisableButtons();

        usedWords = new List<string>();
        enemySpawner = GameObject.Find("EnemySpawner").GetComponent<EnemySpawner>();
        playerObject = GameObject.FindWithTag("Player");
        enemyObject = GameObject.FindWithTag("Enemy");

        if (playerObject != null)
        {
            player = playerObject.GetComponent<Player>();
            playerAnimator= player.GetComponent<Animator>();
        }
        else
        {
            Debug.Log("Can't find player in the current scene");
        }

        if (enemyObject != null)
        {
            enemy = enemyObject.GetComponent<Enemy>();
            enemyAnimator = enemy.GetComponent<Animator>();
        }
        else
        {
            Debug.Log("No enemy in the current scene");
        }

        yield return new WaitForSeconds(1f);
        PlayerTurn();

    }

    void UpdateTurnIndicator(TURNS nextTurn)
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

                playerAnimator.Play("Attk");
                enemyAnimator.Play("Hit");
                enemy.TakeDamage(GetWordDamage() + player.SendDamage());
              
                battleUIManager.UpdateCharacterHUD();

                string selectedWord = LetterGrid.Instance.GetSelectedWord();
                usedWords.Add(selectedWord);
                
                LetterGrid.Instance.ResetSelectedTiles();

                battleUIManager.DisableButtons();
                if (enemy.GetHealth() > 0)
                {
                    StartCoroutine(EnemyTurn());

                }
                else
                {
                    enemy.Die(3);
                    StartCoroutine(SetupNewEnemy());
                }
               

            }

        }

        else return;
    }


    IEnumerator SetupNewEnemy()
    {
        yield return new WaitForSeconds(3f);

        enemySpawner.CreateEnemy();

        enemyObject = GameObject.FindWithTag("Enemy");

        if (enemyObject != null)
        {
            battleUIManager.SetUpCharacterInfo();
            enemy = enemyObject.GetComponent<Enemy>();
            enemyAnimator=enemy.GetComponent<Animator>();
            Debug.Log("New enemy spawned!!");
            PlayerTurn();
        }
        else
        {
            StartCoroutine(EndGame());
        }

    }

    public void OnScrambleButton()
    {
        if (turn == TURNS.PlayerTurn)
        {
            playerAnimator.Play("hit");
            enemyAnimator.Play("Attk");
            Debug.Log("Scrambleee!");
            LetterGrid.Instance.ScrambleLetter();
            battleUIManager.DisableButtons();
            StartCoroutine(EnemyTurn());
        }
        else return;
    }

    void PlayerTurn()
    {
        UpdateTurnIndicator(TURNS.PlayerTurn);

        if (turn == TURNS.PlayerTurn)
        {
            battleUIManager.EnableButtons();
        }
    }

    IEnumerator EnemyTurn()
    {
        UpdateTurnIndicator(TURNS.EnemyTurn);

        if (turn == TURNS.EnemyTurn)
        {
            playerAnimator.Play("hit");
            enemyAnimator.Play("Attk");
            yield return new WaitForSeconds(0.5f);
            
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
                StartCoroutine(EndGame());
            }

        }

    }

    IEnumerator EndGame()
    {
        yield return new WaitForSeconds(0.5f);
        UpdateTurnIndicator(TURNS.Victory);
        SceneManager.LoadScene("Victory");
        
    }

    

}
