using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LoadMap : MonoBehaviour
{
   
    public int currentLevel = 1;
    public GameObject chooseLevel;
    public TMP_Text textLevel;
    private string sceneName;
    private bool canLoadMap = false;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Map1")|| other.CompareTag("Map2") || other.CompareTag("Map3") || other.CompareTag("Map4") || other.CompareTag("Map5") )
        {
            sceneName = other.tag;
            if (chooseLevel != null)
            {
                chooseLevel.SetActive(true);
            }
            canLoadMap = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Map1") || other.CompareTag("Map2") || other.CompareTag("Map3") || other.CompareTag("Map4") || other.CompareTag("Map5"))
        {
            
            if (chooseLevel != null)
            {
                chooseLevel.SetActive(false);
            }
            currentLevel = 1;
        }
        canLoadMap= false;
    }
    
    void Update()
    {
        if (canLoadMap && (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Submit")))
        {
           LoadMapScene();
        }
    }

    public void LoadMapScene()
    {
        LoadMapWithLevel(currentLevel);
    }
    public void LoadMapWithLevel(int difficultyLevel)
    {
        // Save the difficulty level to PlayerPrefs
        PlayerPrefs.SetInt("DifficultyLevel", difficultyLevel);
        PlayerPrefs.Save();

        // Load Scene2
        SceneManager.LoadScene(sceneName);
    }
    public void AddLevel(int level)
    {
        currentLevel += level;
        textLevel.text = currentLevel.ToString();
    }
}
