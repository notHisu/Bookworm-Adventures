using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level Data", menuName = "Scriptable Objects/Level Data", order = 1)]
public class SOLevelData : ScriptableObject
{
    public string levelName;
    public string levelDescription;
    public List<SOCharacterData> enemyScriptableObjects;
}
