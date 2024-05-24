using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Class to manage the UI in a battle scene
public class BattleUIManager : MonoBehaviour
{
    // Private variables to hold the player and enemy
    private Player player;
    private Enemy enemy;

    // Public variables for the player and enemy names, and the turn indicator
    public TMP_Text playerName;
    public TMP_Text enemyName;
    public TMP_Text turnIndicator;

    // Public variables for the player and enemy health bars
    public Slider playerHP;
    public Slider enemyHP;

    // Private variables for the attack and scramble buttons
    [SerializeField]
    private Button attackButton;

    [SerializeField]
    private Button scrambleButton;

    // Private variable for the current score
    [SerializeField]
    private TMP_Text currentScore;

    // Method to set up the player and enemy info in the UI
    public void SetUpCharacterInfo()
    {
        // Find the player and enemy in the scene
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        enemy = GameObject.FindWithTag("Enemy").GetComponent<Enemy>();

        // Set the player and enemy names and health bars
        playerName.text = player.GetName();
        playerHP.maxValue = (float)player.GetMaxHealth();
        playerHP.value = (float)player.GetHealth();

        enemyName.text = enemy.GetName();
        enemyHP.maxValue = (float)enemy.GetMaxHealth();
        enemyHP.value = (float)enemy.GetHealth();
    }

    // Method to update the turn indicator text
    public void UpdateTurnIndicator(string turn)
    {
        turnIndicator.text = turn;
    }

    // Method to update the player and enemy HUD if any changes are made
    public void UpdateCharacterHUD()
    {
        // Find the player and enemy in the scene
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        enemy = GameObject.FindWithTag("Enemy").GetComponent<Enemy>();

        // Update the player and enemy health bars and the current score
        playerHP.value = (float)player.GetHealth();
        enemyHP.value = (float)enemy.GetHealth();
        currentScore.text = ScoreManager.Instance.GetCurrentScore().ToString();
    }

    // Method to disable the attack and scramble buttons
    public void DisableButtons()
    {
        // Get the color blocks of the buttons
        ColorBlock attackButtonCB = attackButton.colors;
        ColorBlock scrambleButtonCB = scrambleButton.colors;

        // Disable the buttons and set their color multipliers to 1
        attackButton.enabled = false;
        attackButtonCB.colorMultiplier = 1;
        attackButton.colors = attackButtonCB;

        scrambleButton.enabled = false;
        scrambleButtonCB.colorMultiplier = 1;
        scrambleButton.colors = scrambleButtonCB;
    }

    // Method to enable the attack and scramble buttons
    public void EnableButtons()
    {
        // Get the color blocks of the buttons
        ColorBlock attackButtonCB = attackButton.colors;
        ColorBlock scrambleButtonCB = scrambleButton.colors;

        // Enable the scramble button and set its color multiplier to 3
        // The attack button is not enabled here
        attackButton.enabled = false;
        attackButtonCB.colorMultiplier = 1;
        attackButton.colors = attackButtonCB;

        scrambleButton.enabled = true;
        scrambleButtonCB.colorMultiplier = 3;
        scrambleButton.colors = scrambleButtonCB;
    }
}
