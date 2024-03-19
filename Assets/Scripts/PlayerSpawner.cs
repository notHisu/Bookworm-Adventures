using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    private SOCharacterData playerData;
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        CreatePlayer();
    }

    void CreatePlayer()
    {
        playerData = Object.Instantiate(Resources.Load("ScriptableObjects/PlayerDemo")) as SOCharacterData;
        player = GameObject.Instantiate(playerData.characterPrefab) as GameObject;

        player.transform.SetParent(this.transform);
        player.name = "Player";

        player.GetComponent<ICharacterTemplate>().CharacterStats(playerData);

    }

}
