using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class LoadMap : MonoBehaviour
{
   
    public int currentLevel = 1;
    public GameObject chooseLevel;
    public TMP_Text textLevel;
    public GameObject panelStatus;
    private SheepfoldStatus sheepfoldStatus;
    private string sceneName;
    private bool canLoadMap = false;
    string[] mapTags = { "Map1", "Map2", "Map3", "Map4", "Map5", "SurvivalMap" };
    string[] lockTags = { "Lock1", "Lock2", "Lock3", "Lock4", "Lock5" };
    int tagIndexMap;
    int tagIndexLock;
    private void Start()
    {
        sheepfoldStatus = panelStatus.GetComponent<SheepfoldStatus>();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
       
         tagIndexMap = Array.IndexOf(mapTags, other.tag);
        if (tagIndexMap != -1)  
        {
            sceneName = other.tag;
            if (chooseLevel != null)
            {
                chooseLevel.SetActive(true);
            }
            canLoadMap = true;
            
        }

        tagIndexLock = Array.IndexOf(lockTags, other.tag);
        if (tagIndexLock != -1)
        {
            if (panelStatus != null)
            {
                int count = DataManager.GetSheepCount(tagIndexLock+1);
                float value = (float)DataManager.GetSheepCountTask(tagIndexLock+1) / count;
                sheepfoldStatus.SetTextSheepfold(tagIndexLock + 1);
                sheepfoldStatus.SetTextCount(count);
                sheepfoldStatus.SetSliderValue(value);
                panelStatus.SetActive(true);
            }

        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (Array.Exists(mapTags, tag => tag == other.tag))
        {
            
            if (chooseLevel != null)
            {
                chooseLevel.SetActive(false);
            }
            currentLevel = 1;
        }
        canLoadMap= false;
        if (Array.Exists(lockTags, tag => tag == other.tag))
        {

            if (panelStatus != null)
            {
                panelStatus.SetActive(false);
            }
        }
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
       // PlayerPrefs.SetInt("DifficultyLevel", difficultyLevel);
       DataManager.DifficultyLevel = difficultyLevel;
        PlayerPrefs.Save();

        // Load Scene2
        SceneManager.LoadScene(sceneName);
    }
    public void AddLevel(int level)
    {
        int i = DataManager.GetSheepCount(tagIndexMap+1);
        currentLevel += level;
        DataManager.SheepInNewMap = i;
        if (currentLevel > i/4)
        {
            
            currentLevel = i/4;
            textLevel.text = currentLevel + "(All)";
            return;
        }
        textLevel.text = currentLevel.ToString();
    }
}
