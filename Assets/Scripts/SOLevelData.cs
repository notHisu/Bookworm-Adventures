// Not related to OOP design, but still SOLID.

using System.Collections.Generic;
using UnityEngine;

// Attribute to create a menu option in Unity to create an instance of this ScriptableObject
[CreateAssetMenu(fileName = "Level Data", menuName = "Scriptable Objects/Level Data", order = 1)]
// Class to store data for a level
public class SOLevelData : ScriptableObject
{
    // Name of the level
    public string levelName;

    // Description of the level
    public string levelDescription;

    // List of enemy character data for the level
    public List<SOCharacterData> enemyScriptableObjects;
}
