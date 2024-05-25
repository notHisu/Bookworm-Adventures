using UnityEngine;

// Class to manage the player's character
public class Player : MonoBehaviour, ICharacterTemplate
{
    // Player Information
    private string playerName; // Player's name
    private double maxHealth; // Player's maximum health
    private double health; // Player's current health
    private double attackDamage; // Player's attack damage

    // Player Sounds
    private AudioClip attackSound; // Sound played when the player attacks
    private AudioClip gotHitSound; // Sound played when the player gets hit
    private AudioSource audioSource; // AudioSource component of the player

    // Player Animation
    private Animator animator; // Animator component of the player
    private string attackAnimation = "Attack"; // Name of the attack animation
    private string gotHitAnimation = "Hit"; // Name of the got hit animation

    // Method to get the player's name
    public string GetName()
    {
        return playerName;
    }

    // Method to get the player's maximum health
    public double GetMaxHealth()
    {
        return maxHealth;
    }

    // Method to get the player's current health
    public double GetHealth()
    {
        return health;
    }

    // Method to set the player's stats
    public void CharacterStats(SOCharacterData characterData)
    {
        // Set the player's stats from the character data
        playerName = characterData.characterName;
        health = characterData.health;
        maxHealth = characterData.maxHealth;
        attackDamage = characterData.attackDamage;
        attackSound = characterData.attackSound;
        gotHitSound = characterData.gotHitSound;
        // Get the AudioSource component
        audioSource = GetComponent<AudioSource>();
        // Get the Animator component
        animator = GetComponent<Animator>();
    }

    // Method to send damage to an enemy
    public double SendDamage()
    {
        // Play the attack animation
        AnimationManager.Instance.PlayAnimation(animator, attackAnimation);
        // Play the attack sound
        SoundManager.Instance.PlaySound(audioSource, attackSound);
        // Return the attack damage
        return attackDamage;
    }

    // Method to take damage from an enemy
    public void TakeDamage(double incomingDamage)
    {
        // Play the got hit animation
        AnimationManager.Instance.PlayAnimation(animator, gotHitAnimation);
        // Play the got hit sound
        SoundManager.Instance.PlaySound(audioSource, gotHitSound);
        // Subtract the incoming damage from the player's health
        health -= incomingDamage;
        // Log the player's current health
        Debug.Log("Player current health: " + health);
    }

    // Method to destroy the player's character when it dies
    public void Die(float time = 0)
    {
        // If the player's health is 0 or less, destroy the player's character
        if (health <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    public void Heal()
    {
        health += 5;
        if (health >= maxHealth)
        {
            health = maxHealth;
        }
    }
}
