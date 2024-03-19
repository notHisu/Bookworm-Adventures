using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacterTemplate 
{
    int SendDamage();
    void TakeDamage(int incomingDamage);
    void CharacterStats(SOCharacterData characterData);
}
