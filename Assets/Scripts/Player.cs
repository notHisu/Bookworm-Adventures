using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, ICharacterTemplate
{
    private string playerName;
    private double maxHealth;
    private double health;

    public string GetName()
    {
        return playerName;
    }

    public double GetMaxHealth()
    {
        return maxHealth;
    }

    public double GetHealth()
    {
        return health;
    }

    public void CharacterStats(SOCharacterData characterData)
    {
        playerName = characterData.characterName;
        health = characterData.health;
        maxHealth = characterData.maxHealth;
    }

    public double SendDamage()
    {
        return 0;
    }


    public void TakeDamage(double incomingDamage)
    {
        
        health -= incomingDamage;
        Debug.Log("Player current health: " + health);
    }

    public void Die()
    {
        if (health <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
