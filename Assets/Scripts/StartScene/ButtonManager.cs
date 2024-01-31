using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
   
    public GameObject objectMenu;
    public GameObject objectSettingsMenu;

    
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

   
    public void SetObjectMenuActive()
    {
        if (objectMenu != null)
        {
            objectMenu.SetActive(true);
            objectSettingsMenu.SetActive(false);
        }
    }

    
    public void SetSettingsMenuActive()
    {
        if (objectSettingsMenu != null)
        {
            objectSettingsMenu.SetActive(true);
            objectMenu.SetActive(false);
        }
    }
    public void QuitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
