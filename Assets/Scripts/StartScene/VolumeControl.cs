using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    public Slider musicSlider;
    public Slider soundSheepSlider;

    private void Start()
    {
        musicSlider.value = AudioManager.instance.musicVolume;
        soundSheepSlider.value = AudioManager.instance.sheepVolume;
    }

    public void SetMusicVolume()
    {
        AudioManager.instance.SetMusicVolume(musicSlider.value);
    }

    
    public void SetSheepVolume()
    {
        AudioManager.instance.SetSoundVolume(soundSheepSlider.value);
    }
}
