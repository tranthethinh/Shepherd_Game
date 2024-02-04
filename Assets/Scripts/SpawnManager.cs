using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public int difficultyLevel;
    public LevelData levelData; 
    public GameObject sheepPrefab;
    public List<GameObject> sheepList = new List<GameObject>();
    public GameObject wolfPrefab;
    public GameObject dogPrefab;
    public Transform dogPos;
    public Transform sheepPos;
    public Transform wolfPos;
    void Start()
    {
        difficultyLevel = PlayerPrefs.GetInt("DifficultyLevel", 1);
        int numberInList = sheepList.Count;
        SpawnSheep(difficultyLevel);
        SpawnSpecialSheep();
        int numWolf = difficultyLevel / 2;
        StartCoroutine(SpawnWolves(numWolf));
        //SpawnDog();
    }
    public int GetNumberOfSheep()
    {
        return difficultyLevel*4 + difficultyLevel/4+1;
    }
    void SpawnSheep(int level)
    {
        for (int i = 0; i < level*4; i++)
        {
            Instantiate(sheepPrefab, sheepPos.position, Quaternion.identity);
        }
    }
    void SpawnSpecialSheep()
    {
        int numOfSpecialSheep = difficultyLevel / 4+1;
        int typeOfSheepInList = difficultyLevel % 4-1;
        for(int i = 0; i < numOfSpecialSheep; i++)
        {
            Instantiate(sheepList[typeOfSheepInList],sheepPos.position, Quaternion.identity);
        }
    }
    IEnumerator SpawnWolves(int numWolf)
    {
        for (int i = 0; i < numWolf; i++)
        {
            Instantiate(wolfPrefab, wolfPos.position, Quaternion.identity);
            yield return new WaitForSeconds(2f); // 2 seconds delay
        }
    }
    void SpawnDog()
    {
        Instantiate(dogPrefab, dogPos.position, Quaternion.identity);
    }
}
