using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CHARACTER_TYPE
{
    Player, Enemy
}

[CreateAssetMenu(fileName = "Character Data", menuName = "Scriptable Objects/Character Data", order = 1)]
public class SOCharacterData : ScriptableObject
{
    public string characterName;
    public CHARACTER_TYPE characterType;
    public string characterDescription;
    // public Sprite characterSprite;
    public double health;
    public double maxHealth;

    // For enemy only, 0 if is player
    public double attackDamage; 

    public GameObject characterPrefab;

}
