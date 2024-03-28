using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, ICharacterTemplate
{
    private string playerName;
    private double maxHealth;
    private double health;
    private double attackDamage;

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
        attackDamage = characterData.attackDamage;
    }

    public double SendDamage()
    {
        return attackDamage;
    }


    public void TakeDamage(double incomingDamage)
    {

        health -= incomingDamage;
        Debug.Log("Player current health: " + health);
    }

    public void Die(float time = 0)
    {
        if (health <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
