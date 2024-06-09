using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GameManagerS : MonoBehaviour
{
    private int numSheepSaved = 0;
    private int numSheepDied = 0;
    public TextMeshProUGUI sheepStatusText;
    public TextMeshProUGUI winLoseText;
    public TextMeshProUGUI numWinLoseText;
    public int difficultyLevel;
    public GameObject winWindow;
    void Start()
    {
        UpdateText();
        difficultyLevel = PlayerPrefs.GetInt("DifficultyLevel", 1);
    }
    public void IncrementSheepSaved()
    {
        numSheepSaved++;
        UpdateText();
        if(numSheepSaved > difficultyLevel*10)
        {
            winLoseText.text = "Win";
            numWinLoseText.text = "Saved: " + numSheepSaved;
            winWindow.SetActive(true);
            Time.timeScale = 0;
        }
    }

    // Method to increment the dead sheep count
    public void IncrementSheepDied()
    {
        numSheepDied++;
        UpdateText();
        if (numSheepDied > difficultyLevel)
        {
            winLoseText.text = "Lose";
            numWinLoseText.text = "Die: " + numSheepDied;
            winWindow.SetActive(true);
            Time.timeScale = 0;
        }
    }
    void UpdateText()
    {
        // Update TextMeshPro text with the current counts
        sheepStatusText.text = "Saved: " + numSheepSaved +  "\n" +"Died: " + numSheepDied;
    }
}
