using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacterTemplate
{
    double SendDamage();
    void TakeDamage(double incomingDamage);
    void CharacterStats(SOCharacterData characterData);
    void Die(float time=0);

}
