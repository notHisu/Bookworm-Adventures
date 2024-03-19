using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, ICharacterTemplate
{
    private string playerName;
    private int maxHealth;
    private int health;

    public void CharacterStats(SOCharacterData characterData)
    {
        playerName = characterData.name;
        health = characterData.health;
        maxHealth = characterData.maxHealth;
    }

    public int SendDamage()
    {
        return 0;
    }

    public void TakeDamage(int incomingDamage)
    {
        health -= incomingDamage;
    }
}
