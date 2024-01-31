using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public Vector2 boxSize = new Vector2(6f, 6f);
    public int numberOfSheep;
    SpawnManager spawnManager;
    public GameObject winPanel;
    public TMP_Text numSheepWin;
    private void Start()
    {
        spawnManager = GetComponent<SpawnManager>();
        numberOfSheep = spawnManager.GetNumberOfSheep();
    }
    void Update()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll((Vector2)transform.position, boxSize, 0f);

        int sheepCount = 0;

        foreach (Collider2D collider in colliders)
        {
            // Check if the collider has the "sheep" tag
            if (collider.CompareTag("Sheep"))
            {
                sheepCount++;
            }
        }
        int numSheepRemaining = CountSheep();
        if (sheepCount >= numSheepRemaining) 
        {
            Debug.Log("win");
            winPanel.SetActive(true);
            Time.timeScale = 0;
            numSheepWin.text = "Sheep: "+numSheepRemaining + "/" + numberOfSheep;
        }
    }

    // draw the box gizmo in the Scene view for visualization
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube((Vector2)transform.position, boxSize);
    }
    public void LoadMainScene()
    {
        SceneManager.LoadScene(0);
    }
    int CountSheep()
    {
        // Find all GameObjects with the "Sheep" tag
        GameObject[] sheepObjects = GameObject.FindGameObjectsWithTag("Sheep");

        // Return the count of the sheepObjects array
        return sheepObjects.Length;
    }
}
