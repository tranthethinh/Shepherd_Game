using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SpawnSheepInMain : MonoBehaviour
{
    public List<Transform> positions = new List<Transform>();
    public GameObject sheepPrefab;
    public List<GameObject> lockObject;
    private void Start()
    {
        for (int i = 0; i < positions.Count; i++)
        {
            int num = DataManager.GetSheepCount(i+1);
            if(num > 0)
            {
                //lockObject[i].SetActive(false);
                SpriteRenderer spriteRenderer = lockObject[i].GetComponent<SpriteRenderer>();
                spriteRenderer.enabled = false;
                for (int j = 0;j< num; j++)
                {
                    Instantiate(sheepPrefab, positions[i].position, Quaternion.identity);
                }
                
            }
        }
    }
}
