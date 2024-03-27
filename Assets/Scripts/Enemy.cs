using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, ICharacterTemplate
{
    private string enemyName;
    private double maxHealth;
    private double health;
    private double attackDamage;

    public string GetName()
    {
        return enemyName;
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
        enemyName = characterData.characterName;
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
        Debug.Log("Enemy current health: " + health);

    }

    public void Die()
    {
        if (health <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
