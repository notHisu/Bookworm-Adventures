using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Interface to define a common set of methods for character classes
public interface ICharacterTemplate
{
    // Method to send damage to another character
    double SendDamage();

    // Method to take damage from another character
    void TakeDamage(double incomingDamage);

    // Method to set the character's stats
    void CharacterStats(SOCharacterData characterData);

    // Method to destroy the character when it dies
    void Die(float time = 0);
}
