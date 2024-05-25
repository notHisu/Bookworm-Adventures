using System.Collections.Generic;
using UnityEngine;

public class EnemyFactory
{
    public GameObject CreateEnemy(SOCharacterData enemyData)
    {
        // Instantiate the enemy GameObject from the Resources folder
        GameObject enemy =
            Object.Instantiate(enemyData.characterPrefab)
            as GameObject;

        // Set the enemy's character data
        enemy.GetComponent<ICharacterTemplate>().CharacterStats(enemyData);

        // Perform additional setup for different enemy type

        // Return the created enemy
        return enemy;
    }
}
