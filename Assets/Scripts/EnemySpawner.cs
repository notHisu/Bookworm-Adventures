using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private SOLevelData levelData;
    private SOCharacterData enemyData;
    private GameObject enemy;
    private List<SOCharacterData> enemyList;
    private int enemyCount;

    // Start is called before the first frame update
    void Start()
    {
        LoadLevelData();
    }

    void LoadLevelData()
    {
        levelData = Object.Instantiate(Resources.Load("ScriptableObjects/Levels/Level1")) as SOLevelData;
        enemyCount = levelData.enemyScriptableObjects.Count;
        // Debug.Log("Enemy count: " + enemyCount);
        enemyList = levelData.enemyScriptableObjects;
        CreateEnemy();
    }

    public void CreateEnemy()
    {
        if (enemyList.Count > 0)
        {
            enemyData = enemyList[0]; // Get the first enemy data
            enemy = GameObject.Instantiate(enemyData.characterPrefab) as GameObject;

            enemy.transform.SetParent(this.transform);
            enemy.name = "Enemy";

            enemy.GetComponent<ICharacterTemplate>().CharacterStats(enemyData);

            enemyList.RemoveAt(0); // Remove the used enemy data from the list
        }
        else
        {
            Debug.Log("No more enemies to spawn!"); // Handle the case where the list is empty
        }
    }

}
