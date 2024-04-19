using System.Collections.Generic;
using UnityEngine;

// Class to spawn enemies
public class EnemySpawner : MonoBehaviour
{
    // Level data for the current level
    private SOLevelData levelData;

    // Character data for the enemy to spawn
    private SOCharacterData enemyData;

    // The enemy GameObject to spawn
    private GameObject enemy;

    // List of enemy character data
    private List<SOCharacterData> enemyList;

    // Count of enemies to spawn
    private int enemyCount;

    // Method called before the first frame update
    void Start()
    {
        // Load the level data
        LoadLevelData();
    }

    // Method to load the level data
    void LoadLevelData()
    {
        // Instantiate the level data from the Resources folder
        levelData =
            Object.Instantiate(Resources.Load("ScriptableObjects/Levels/Level1")) as SOLevelData;
        // Get the count of enemies to spawn
        enemyCount = levelData.enemyScriptableObjects.Count;
        // Get the list of enemy character data
        enemyList = levelData.enemyScriptableObjects;
        // Create an enemy
        CreateEnemy();
    }

    // Method to create an enemy
    public void CreateEnemy()
    {
        // If there are enemies to spawn
        if (enemyList.Count > 0)
        {
            // Get a random enemy data from the list
            enemyData = enemyList[Random.Range(0, enemyCount)];
            // Instantiate the enemy GameObject from the enemy data
            enemy = GameObject.Instantiate(enemyData.characterPrefab) as GameObject;

            // Set the enemy's parent to this GameObject
            enemy.transform.SetParent(this.transform);
            // Set the enemy's name
            enemy.name = "Enemy";

            // Set the enemy's stats
            enemy.GetComponent<ICharacterTemplate>().CharacterStats(enemyData);

            // Uncomment the following line to remove the used enemy data from the list
            // enemyList.RemoveAt(0);
        }
        else
        {
            // Log a message if there are no more enemies to spawn
            Debug.Log("No more enemies to spawn!");
        }
    }
}
