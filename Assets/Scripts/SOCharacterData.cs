// Not related to OOP design, but still SOLID.

using UnityEngine;

// Enum to define the type of the character
public enum CHARACTER_TYPE
{
    Player,
    Enemy
}

// Attribute to create a menu option in Unity to create an instance of this ScriptableObject
[CreateAssetMenu(
    fileName = "Character Data",
    menuName = "Scriptable Objects/Character Data",
    order = 1
)]

// Class to store data for a character
public class SOCharacterData : ScriptableObject
{
    // Name of the character
    public string characterName;

    // Type of the character
    public CHARACTER_TYPE characterType;

    // Description of the character
    public string characterDescription;

    // Health of the character
    public double health;

    // Maximum health of the character
    public double maxHealth;

    // Attack damage of the character, used for enemies only, 0 if it's a player
    public double attackDamage;

    // Sound played when the character attacks
    public AudioClip attackSound;

    // Sound played when the character gets hit
    public AudioClip gotHitSound;

    // Prefab of the character
    public GameObject characterPrefab;
}
