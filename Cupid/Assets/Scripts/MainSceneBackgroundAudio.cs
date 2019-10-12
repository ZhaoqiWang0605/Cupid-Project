using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneBackgroundAudio : MonoBehaviour
{
    public AudioClip musicClip;
    public AudioSource musicSource;

    // Start is called before the first frame update
    void Start()
    {
        musicSource.clip = musicClip;
        musicSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
