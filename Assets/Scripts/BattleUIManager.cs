using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.UI;

public class BattleUIManager : MonoBehaviour
{
    private Player player;
    private Enemy enemy;

    public TMP_Text playerName;
    public TMP_Text enemyName;
    public TMP_Text turnIndicator;

    public Slider playerHP;
    public Slider enemyHP;

    [SerializeField]
    private Button attackButton;

    [SerializeField]
    private Button scrambleButton;

    // Setup UI for player and enemy in the scene
    public void SetUpCharacterInfo()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        enemy = GameObject.FindWithTag("Enemy").GetComponent<Enemy>();

        playerName.text = player.GetName();
        playerHP.maxValue = (float)player.GetMaxHealth();
        playerHP.value = (float)player.GetHealth();

        enemyName.text = enemy.GetName();
        enemyHP.maxValue = (float)enemy.GetMaxHealth();
        enemyHP.value = (float)enemy.GetHealth();
    }

    // Update turn indicator text
    public void UpdateTurnIndicator(string turn)
    {
        turnIndicator.text = turn;
    }

    // Update player and enemy HUD if any changes are made
    public void UpdateCharacterHUD()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        enemy = GameObject.FindWithTag("Enemy").GetComponent<Enemy>();

        playerHP.value = (float)player.GetHealth();
        enemyHP.value = (float)enemy.GetHealth();
    }

    // Disable battle UI
    public void DisableButtons()
    {
        ColorBlock attackButtonCB = attackButton.colors;
        ColorBlock scrambleButtonCB = scrambleButton.colors;

        attackButtonCB.colorMultiplier = 1;
        attackButton.colors = attackButtonCB;
        attackButton.enabled = false;

        scrambleButtonCB.colorMultiplier = 1;
        scrambleButton.enabled = false;
        scrambleButton.colors = scrambleButtonCB;
    }

    // Enable battle UI
    public void EnableButtons()
    {
        ColorBlock attackButtonCB = attackButton.colors;
        ColorBlock scrambleButtonCB = scrambleButton.colors;

        attackButton.enabled = false;
        attackButtonCB.colorMultiplier = 1;
        attackButton.colors = attackButtonCB;

        scrambleButton.enabled = true;
        scrambleButtonCB.colorMultiplier = 3;
        scrambleButton.colors = scrambleButtonCB;
    }
}
