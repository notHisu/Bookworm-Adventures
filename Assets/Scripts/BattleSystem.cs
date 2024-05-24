using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum TURNS
{
    Start,
    PlayerTurn,
    Processing,
    EnemyTurn,
    Victory,
    //Defeated
}

public class BattleSystem : MonoBehaviour
{
    // Singleton instance of the BattleSytem class
    private static BattleSystem instance;

    // Dictionary to store the damage values of each word length
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

    // Current turn of the battle
    [SerializeField]
    private TURNS turn;

    // Reference to the BattleUIManager script
    [SerializeField]
    private BattleUIManager battleUIManager;

    // Reference to the EnemySpawner script
    private EnemySpawner enemySpawner;

    // Reference to the player and enemy game objects
    private GameObject playerObject;
    private GameObject enemyObject;

    // Reference to the player and enemy scripts
    private Player player;
    private Enemy enemy;

    // Current coroutine
    private Coroutine currentCoroutine;

    // Delay for player and enemy turns
    private float playerTurnDelay = 1f;
    private float enemyTurnDelay = 2f;
    private float processingDelay = 1f;

    // Singleton instance of the BattleSystem class
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

    // Awake method to create the singleton instance
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

    // Coroutine to set up the battle
    IEnumerator Setup()
    {
        // Reset current score
        ScoreManager.Instance.ResetScore();

        // Set the initial state of the battle to Start
        SetState(TURNS.Start);

        // Set up the character info in the UI and disable the UI buttons
        battleUIManager.SetUpCharacterInfo();
        battleUIManager.DisableButtons();

        // Find the EnemySpawner object in the scene and get its EnemySpawner component
        enemySpawner = GameObject.Find("EnemySpawner").GetComponent<EnemySpawner>();

        // Find the player and enemy objects in the scene
        playerObject = GameObject.FindWithTag("Player");
        enemyObject = GameObject.FindWithTag("Enemy");

        // If the player object was found, get its Player component
        if (playerObject != null)
        {
            player = playerObject.GetComponent<Player>();
        }
        else
        {
            // If the player object wasn't found, log an error
            Debug.Log("Can't find player in the current scene");
        }

        // If the enemy object was found, get its Enemy component
        if (enemyObject != null)
        {
            enemy = enemyObject.GetComponent<Enemy>();
        }
        else
        {
            // If the enemy object wasn't found, log an error
            Debug.Log("No enemy in the current scene");
        }

        // Wait for a delay before starting the player's turn
        yield return new WaitForSeconds(processingDelay);
        PlayerTurn();
    }

    // Set the current turn in the battle
    void SetState(TURNS nextTurn)
    {
        // Set the current turn to the next turn
        turn = nextTurn;

        // Update the turn indicator in the UI to reflect the new turn
        // The ToString method is used to convert the enum value to a string
        battleUIManager.UpdateTurnIndicator(nextTurn.ToString());
    }

    // Method to get the damage value of the selected word
    double GetWordDamage()
    {
        // Get the value of the selected word from the LetterGrid
        // The Instance property is used to access the singleton instance of the LetterGrid class
        int wordValue = LetterGrid.Instance.GetSelectedWordValue();
        int wordLength = LetterGrid.Instance.GetSelectedWord().Length;

        // Try to get the damage value for the word value from the damageValues dictionary
        if (damageValues.TryGetValue(wordLength, out double value))
        {
            // If the word value is found in the dictionary, return the corresponding damage value
            return value + wordValue;
        }
        else
        {
            // If the word value is not found in the dictionary, return 0
            return 0;
        }
    }

    // Method to handle the player pressing the attack button
    public void OnAttackButton()
    {
        // Check if the damage value of the selected word is greater than 0
        if (GetWordDamage() > 0)
        {
            // Check if it's currently the player's turn
            if (turn == TURNS.PlayerTurn)
            {
                // Add the damage value of the selected word to the player's score
                ScoreManager.Instance.AddScore(GetWordDamage());

                // Log the damage value of the selected word
                Debug.Log("Word damage: " + GetWordDamage());

                // If a coroutine is currently running, stop it
                if (currentCoroutine != null)
                {
                    StopCoroutine(currentCoroutine);
                }

                // Start the PlayerAttack coroutine
                currentCoroutine = StartCoroutine(PlayerAttack());
            }
        }
    }

    // Coroutine to handle the player's attack
    IEnumerator PlayerAttack()
    {
        // If a coroutine is currently running, stop it
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }

        // Make the enemy take damage equal to the damage value of the selected word plus the player's damage
        enemy.TakeDamage(GetWordDamage() + player.SendDamage());

        // Wait for a delay before updating the character HUD in the UI
        yield return new WaitForSeconds(playerTurnDelay);
        battleUIManager.UpdateCharacterHUD();

        // Wait for another delay before resetting the selected tiles in the LetterGrid and disabling the UI buttons
        yield return new WaitForSeconds(processingDelay);
        LetterGrid.Instance.ResetSelectedTiles();
        battleUIManager.DisableButtons();

        // Check if the enemy still has health left
        if (enemy.GetHealth() > 0)
        {
            // If the enemy is still alive, start the EnemyTurn coroutine
            currentCoroutine = StartCoroutine(EnemyTurn());
        }
        else
        {
            // If the enemy is dead, make it die and start the SetupNewEnemy coroutine
            enemy.Die(1);
            currentCoroutine = StartCoroutine(SetupNewEnemy());
        }
    }

    // Coroutine to set up a new enemy
    IEnumerator SetupNewEnemy()
    {
        // If a coroutine is currently running, stop it
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }

        // Heal player by 5
        player.Heal();

        // Wait for a delay before creating a new enemy
        yield return new WaitForSeconds(processingDelay);
        enemySpawner.CreateEnemy();

        // Find the enemy object in the scene
        enemyObject = GameObject.FindWithTag("Enemy");

        // If the enemy object was found, set up its info in the UI and get its Enemy component
        if (enemyObject != null)
        {
            battleUIManager.SetUpCharacterInfo();
            enemy = enemyObject.GetComponent<Enemy>();

            // Log that a new enemy has been spawned and start the player's turn
            Debug.Log("New enemy spawned!!");
            PlayerTurn();
        }
        else
        {
            // If the enemy object wasn't found, start the EndGame coroutine
            currentCoroutine = StartCoroutine(EndGame());
        }
    }

    // Method to handle the player pressing the scramble button
    public void OnScrambleButton()
    {
        // If a coroutine is currently running, stop it
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }

        // Check if it's currently the player's turn
        if (turn == TURNS.PlayerTurn)
        {
            // Log that the scramble button has been pressed
            Debug.Log("Scrambleee!");

            // Scramble the letters in the LetterGrid
            LetterGrid.Instance.ScrambleLetter();

            // Disable the UI buttons
            battleUIManager.DisableButtons();

            // Start the EnemyTurn coroutine
            currentCoroutine = StartCoroutine(EnemyTurn());
        }
        else
        {
            // If it's not the player's turn, do nothing and return
            return;
        }
    }

    // Method to handle the player's turn
    void PlayerTurn()
    {
        // Set the current turn to the player's turn
        SetState(TURNS.PlayerTurn);

        EnableLetterGrid();

        // Check if it's currently the player's turn
        if (turn == TURNS.PlayerTurn)
        {
            // If it is the player's turn, enable the UI buttons
            battleUIManager.EnableButtons();
        }
    }

    // Coroutine to handle the enemy's turn
    IEnumerator EnemyTurn()
    {
        // Set the current turn to the enemy's turn
        SetState(TURNS.EnemyTurn);

        // Disable LetterGrid
        DisableLetterGrid();

        // Check if it's currently the enemy's turn
        if (turn == TURNS.EnemyTurn)
        {
            // If it is the enemy's turn, make the player take damage equal to the enemy's damage
            player.TakeDamage(enemy.SendDamage());

            // Wait for a delay before updating the character HUD in the UI
            yield return new WaitForSeconds(enemyTurnDelay);
            battleUIManager.UpdateCharacterHUD();

            // Wait for another delay before checking if the player still has health left
            yield return new WaitForSeconds(processingDelay);

            // If the player still has health left, start the player's turn
            if (player.GetHealth() > 0)
            {
                PlayerTurn();
            }
            else
            {
                // If the player is dead, make it die and start the EndGame coroutine
                player.Die();
                currentCoroutine = StartCoroutine(EndGame());
            }
        }
    }

    void DisableLetterGrid()
    {
        LetterGrid.Instance.enabled = false;
    }

    void EnableLetterGrid()
    {
        LetterGrid.Instance.enabled = true;
    }

    // Coroutine to handle the end of the game
    IEnumerator EndGame()
    {
        // If a coroutine is currently running, stop it
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }

        // Wait for a delay before setting the current turn to Victory and loading the Victory scene
        yield return new WaitForSeconds(processingDelay);
        SetState(TURNS.Victory);
        SceneManager.LoadScene("Victory");
    }

}
