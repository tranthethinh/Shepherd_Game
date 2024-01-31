using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    private AudioSource musicSource;

    private void Start()
    {
        
        musicSource = GetComponent<AudioSource>();
    }
    private void Update()
    {
        musicSource.volume = AudioManager.instance.musicVolume;
    }
    public void PlayMusic()
    {
        
        musicSource.volume = AudioManager.instance.musicVolume;

       
        musicSource.Play();
    }
}

