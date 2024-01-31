using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SheepVolume : MonoBehaviour
{
    
    public Slider soundSheepSlider;

    private void Start()
    {
        
        soundSheepSlider.value = AudioManager.instance.sheepVolume;
    }
    public void SetSheepVolume()
    {
        AudioManager.instance.SetSoundVolume(soundSheepSlider.value);
    }
}
