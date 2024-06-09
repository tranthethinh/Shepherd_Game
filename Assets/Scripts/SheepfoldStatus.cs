using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SheepfoldStatus : MonoBehaviour
{

    public TMP_Text textsheepfold;
    public TMP_Text textCount;
    public TMP_Text textPercent;
    public Slider slider;
    
    public void SetTextSheepfold(int i)
    {
        textsheepfold.text = "Sheepfold " + i;
    }
    public void SetTextCount(int i)
    {
        textCount.text = "Count: " + i;
    }
    public void SetSliderValue(float i)
    {
        slider.value = i;
        textPercent.text = (int)(i * 100)+"%";
    }
}
