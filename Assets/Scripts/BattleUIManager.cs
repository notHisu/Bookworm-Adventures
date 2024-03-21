using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
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

    public void UpdateTurnIndicator(string turn)
    {
        turnIndicator.text = turn;

    }

    public void UpdateCharacterHUD()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        enemy = GameObject.FindWithTag("Enemy").GetComponent<Enemy>();

        playerHP.value = (float)player.GetHealth();
        enemyHP.value = (float)enemy.GetHealth();

    }

    public void DisableButtons ()
    {
        attackButton.enabled = false;
        scrambleButton.enabled = false;
    }

    public void EnableButtons ()
    {
        attackButton.enabled = true;
        scrambleButton.enabled = true;
    }

}
