using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class to spawn the player character
public class PlayerSpawner : MonoBehaviour
{
    // Data for the player character
    private SOCharacterData playerData;

    // The player character object
    private GameObject player;

    // Method called before the first frame update
    void Start()
    {
        // Create the player character
        CreatePlayer();
    }

    // Method to create the player character
    void CreatePlayer()
    {
        // Load the player data from a scriptable object in the Resources folder
        playerData =
            Object.Instantiate(Resources.Load("ScriptableObjects/PlayerDemo")) as SOCharacterData;
        // Instantiate the player character prefab from the player data
        player = GameObject.Instantiate(playerData.characterPrefab) as GameObject;

        // Set the parent of the player character to this object
        player.transform.SetParent(this.transform);
        // Set the name of the player character
        player.name = "Player";

        // Set the stats of the player character from the player data
        player.GetComponent<ICharacterTemplate>().CharacterStats(playerData);
    }
}
