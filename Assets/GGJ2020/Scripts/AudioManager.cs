using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : SingletonController<AudioManager>
{
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource soundEffectSource;

    [SerializeField] AudioClip repairClip;
    [SerializeField] AudioClip menuScreenMusic;
    [SerializeField] AudioClip gameScreenMusic;
    [SerializeField] AudioClip gameOverScreenMusic;

    private static AudioManager inst;


    private void Awake() {
        //if (inst == null) {
        //    inst = this;
        //    DontDestroyOnLoad(gameObject);
        //}
        //else {
        //    Destroy(this.gameObject);
        //}
    }

    public void PlayRepair() 
    { 
        soundEffectSource.clip = repairClip; 
        soundEffectSource.Play(); 
    }

    public void PlayMenuScreenMusic()
    {
        musicSource.clip = menuScreenMusic;
        musicSource.Play();
    }

    public void PlayGameScreenMusic()
    {
        musicSource.clip = gameScreenMusic;
        musicSource.Play();
    }
    public void PlayGameOverScreenMusic()
    {
        musicSource.clip = gameOverScreenMusic;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }
}
