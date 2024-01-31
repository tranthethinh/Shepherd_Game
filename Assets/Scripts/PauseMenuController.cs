using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    public GameObject pauseMenu;

   
    void Start()
    {
        Time.timeScale = 1.0f;
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(false);
        }
    }

   
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
    }

    public void OnPauseButtonClicked()
    {
        TogglePauseMenu();
    }

    void TogglePauseMenu()
    {
       
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(!pauseMenu.activeSelf);
        }

       
        if (pauseMenu.activeSelf)
        {
            Time.timeScale = 0f; // Pause the game
        }
        else
        {
            Time.timeScale = 1f; // Resume the game
        }
    }

    public void LoadScene(string sceneName)
    {
        Time.timeScale = 1f; // Resume the game
        SceneManager.LoadScene(sceneName);
    }
}
