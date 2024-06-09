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
    private int numberOfSheep;
    int numberOfSpecialSheep;
    void Start()
    {
        numberOfSheep = DataManager.SheepInNewMap;
        numberOfSpecialSheep = Random.Range(0, numberOfSheep / 4);
        
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
        return numberOfSheep;
        //return difficultyLevel*4 + difficultyLevel/4+1;
    }
    void SpawnSheep(int level)
    {
        int num = numberOfSheep - numberOfSpecialSheep;
        for (int i = 0; i < num; i++)
        {
            Instantiate(sheepPrefab, sheepPos.position, Quaternion.identity);
        }
    }
    void SpawnSpecialSheep()
    {
        //int numOfSpecialSheep = difficultyLevel / 4+1;
        //int typeOfSheepInList = difficultyLevel % 4;
        for(int i = 0; i < numberOfSpecialSheep; i++)
        {
            Instantiate(sheepList[Random.Range(0,4)],sheepPos.position, Quaternion.identity);
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
