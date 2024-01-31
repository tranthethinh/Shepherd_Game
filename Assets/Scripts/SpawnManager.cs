using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public int difficultyLevel;
    public LevelData levelData; 
    public GameObject sheepPrefab;
    public GameObject wolfPrefab;
    public GameObject dogPrefab;
    public Transform dogPos;
    public Transform sheepPos;
    public Transform wolfPos;
    void Start()
    {
        difficultyLevel = PlayerPrefs.GetInt("DifficultyLevel", 1);

        SpawnSheep(difficultyLevel);
        SpawnWolves(difficultyLevel);
        //SpawnDog();
    }
    public int GetNumberOfSheep()
    {
        return difficultyLevel*4;
    }
    void SpawnSheep(int level)
    {
        for (int i = 0; i < level*4; i++)
        {
            Instantiate(sheepPrefab, sheepPos.position, Quaternion.identity);
        }
    }

    void SpawnWolves(int level)
    {
        int numWolf = level / 2;
        for (int i = 0; i < numWolf; i++)
        {
            Instantiate(wolfPrefab, wolfPos.position, Quaternion.identity);
        }
    }
    void SpawnDog()
    {
        Instantiate(dogPrefab, dogPos.position, Quaternion.identity);
    }
}
