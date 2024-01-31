using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/Level Data", order = 1)]
public class LevelData : ScriptableObject
{
    public int level;
    public int numberOfSheep;
    public int numberOfWolves;
}

