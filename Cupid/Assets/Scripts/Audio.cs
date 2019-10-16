using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour
{
    public AudioClip musicClip;
    public AudioSource musicSource;
    public bool isBackground;

    // Start is called before the first frame update
    void Start()
    {
        musicSource.clip = musicClip;
        if (isBackground)
        {
            musicSource.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayMusic()
    {
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Pause();
    }
}
