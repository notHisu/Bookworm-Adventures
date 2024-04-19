using UnityEngine;

public class Enemy : MonoBehaviour, ICharacterTemplate
{
    private string enemyName;
    private double maxHealth;
    private double health;
    private double attackDamage;
    private AudioClip attackSound;
    private AudioClip gotHitSound;
    private AudioSource audioSource;
    private Animator animator;
    private string attackAnimation = "Attack";
    private string gotHitAnimation = "Hit";

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
        attackSound = characterData.attackSound;
        gotHitSound = characterData.gotHitSound;
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

    public double SendDamage()
    {
        AnimationManager.Instance.PlayAnimation(animator, attackAnimation);
        SoundManager.Instance.PlaySound(audioSource, attackSound);
        return attackDamage;
    }

    public void TakeDamage(double incomingDamage)
    {
        AnimationManager.Instance.PlayAnimation(animator, gotHitAnimation);
        SoundManager.Instance.PlaySound(audioSource, gotHitSound);
        health -= incomingDamage;
        Debug.Log("Enemy current health: " + health);
    }

    public void Die(float time = 0)
    {
        if (health <= 0)
        {
            Destroy(this.gameObject, time);
        }
    }
}
