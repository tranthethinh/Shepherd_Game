using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SpawnSheep : MonoBehaviour
{
    public GameObject center; // Center of the circle
    public float rangeSpawn = 10f; // Radius of the circle
    private float timeBetweenWaves = 5f; // Delay between waves
    private float minObjectsToSpawn = 1;
    private float maxObjectsToSpawn = 2;
    public GameObject[] EnemyNormal;
    public static int currentMaxSheep = 2;
    private Vector2 spawnPosition;
    GameManagerS gameManager;
    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManagerS>();
        center = GameObject.FindWithTag("Dog");
        StartCoroutine(SpawnWaves());
    }

    IEnumerator SpawnWaves()
    {
        while (true)
        {
            for (int wave = 0; wave < 3; wave++)
            {


                int numberOfObjects = Random.Range((int)minObjectsToSpawn, (int)maxObjectsToSpawn);

                for (int i = 0; i < numberOfObjects; i++)
                {
                    float angle = Random.Range(0f, 360f);
                    float delay = Random.Range(0f, 2f);
                    int numRandom = Random.Range(0, 2);
                    yield return new WaitForSeconds(delay);
                    if (center != null)
                        spawnPosition = center.transform.position + new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad) * rangeSpawn, Mathf.Sin(angle * Mathf.Deg2Rad) * rangeSpawn, 0f);

                    /*if (currentMaxSheep % 3 == 0)
                    {
                        if (Random.Range(0, 2) == 0)
                        {
                            Instantiate(EnemySpeed, spawnPosition, Quaternion.identity);
                        }
                        else
                        {
                            Instantiate(EnemyNormal[numRandom], spawnPosition, Quaternion.identity);
                        }
                    }
                    else
                    {*/
                    GameObject newSheep = Instantiate(EnemyNormal[numRandom], spawnPosition, Quaternion.identity);

                    // Start a coroutine to check if the sheep is destroyed after 20 seconds
                    StartCoroutine(CheckSheepAfterDelay(newSheep));
                }

                yield return new WaitForSeconds(timeBetweenWaves);
            }

            currentMaxSheep++;
            
            maxObjectsToSpawn = currentMaxSheep;

        }
    }
    IEnumerator CheckSheepAfterDelay(GameObject sheepToCheck)
    {
        yield return new WaitForSeconds(20f);

        // Check if the sheep is still alive after 20 seconds
        if (sheepToCheck != null)
        {
            // The sheep is not destroyed, increment the death count
            gameManager.IncrementSheepDied();
            Destroy(sheepToCheck); // Destroy the sheep after counting it as dead
        }
    }
}
