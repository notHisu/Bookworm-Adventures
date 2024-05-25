using System.Collections.Generic;
using UnityEngine;

// Class to spawn enemies
public class EnemySpawner : MonoBehaviour
{
    // Level Information
    [SerializeField]
    private string levelName; // Name of the current level
    private SOLevelData levelData; // Level data for the current level

    // Enemy Information
    private SOCharacterData enemyData; // Character data for the enemy to spawn
    private List<SOCharacterData> enemyList; // List of enemy character data
    private int enemyCount; // Count of enemies to spawn

    // Enemy GameObject
    private GameObject enemy; // The enemy GameObject to spawn

    // Factory for creating enemies
    private EnemyFactory enemyFactory; // The EnemyFactory instance

    // Method called before the first frame update
    void Start()
    {
        enemyFactory = new EnemyFactory();

        // Load the level data
        LoadLevelData(levelName);
    }

    // Method to load the level data
    void LoadLevelData(string name)
    {
        // Instantiate the level data from the Resources folder
        levelData =
            Object.Instantiate(Resources.Load("ScriptableObjects/Levels/" + name)) as SOLevelData;
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
            enemy = enemyFactory.CreateEnemy(enemyData);

            // Set the enemy's parent to this GameObject
            enemy.transform.SetParent(this.transform);
            // Set the enemy's name
            enemy.name = "Enemy";

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
