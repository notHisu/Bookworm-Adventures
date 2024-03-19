using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BattleUIManager : MonoBehaviour
{
    public TMP_Text playerName;
    public TMP_Text enemyName;
    public TMP_Text turnIndicator;

    public Slider playerHP;
    public Slider enemyHP;

    public void SetUpCharacterInfo()
    {
        
    }

    public void UpdateTurnIndicator(string turn)
    {
        turnIndicator.text = turn;
    }

}
