using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Serialization;
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; private set; }
    public  AudioSource soundSource;
    public AudioSource musicSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            
            Destroy(gameObject);
            return;
        }

        soundSource = GetComponent<AudioSource>();
        musicSource = transform.Find("Music").GetComponent<AudioSource>(); 

        LoadVolumesFromPlayerPrefs();
    }



    private void LoadVolumesFromPlayerPrefs()
    {
      
        soundSource.volume = PlayerPrefs.GetFloat("soundVolume", 1);
        musicSource.volume = PlayerPrefs.GetFloat("musicVolume", 0.3f);
    }

    public void PlaySoundEffects(AudioClip sound)
    {
        soundSource.PlayOneShot(sound);
    }
    public void PlayBackground(AudioClip backSound)
    {
      
        musicSource.clip = backSound;

        
        musicSource.loop = true;

       
        musicSource.Play();
    }
    public void ChangeSoundVolume(float value)
    {
        soundSource.volume = value;
    }

    public void ChangeMusicVolume(float value)
    {
        musicSource.volume = value;
    }
}