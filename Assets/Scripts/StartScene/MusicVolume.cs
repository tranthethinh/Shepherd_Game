using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MusicVolume : MonoBehaviour
{
    public Slider musicSlider;
    private void Start()
    {
        musicSlider.value = AudioManager.instance.musicVolume;
    }

    public void SetMusicVolume()
    {
        AudioManager.instance.SetMusicVolume(musicSlider.value);
    }
}
