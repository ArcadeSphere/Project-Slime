using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public GameObject audioManagerPrefab;
    public Slider soundSlider;
    public Slider musicSlider;

    private AudioManager audioManager;
    private float soundVolume = 1f;
    private float musicVolume = 0.3f;

    private void Awake()
    {
        InitializeAudioManager();
        InitializeSliders();
    }

    private void InitializeAudioManager()
    {
      
        if (AudioManager.instance == null)
        {
         
            if (audioManagerPrefab != null)
            {
                GameObject audioManagerInstance = Instantiate(audioManagerPrefab);

              
                audioManagerInstance.transform.SetParent(transform);
                audioManager = audioManagerInstance.GetComponent<AudioManager>();
            }
            else
            {
                Debug.LogError("AudioManagerPrefab field is not set in the GameManager. Drag AudioManagerPrefab into this field in the Inspector.");
            }
        }
        else
        {
            audioManager = AudioManager.instance;
        }
    }

    private void InitializeSliders()
    {
       
        soundSlider.value = audioManager.soundSource.volume;
        musicSlider.value = audioManager.musicSource.volume;

        
        soundSlider.onValueChanged.AddListener(ChangeSoundVolume);
        musicSlider.onValueChanged.AddListener(ChangeMusicVolume);
    }

    public float GetSoundVolume()
    {
        return soundVolume;
    }

    public void SetSoundVolume(float volume)
    {
        soundVolume = volume;
        PlayerPrefs.SetFloat("soundVolume", volume);
        PlayerPrefs.Save();

        // Update AudioManager volume
        if (audioManager != null)
        {
            audioManager.ChangeSoundVolume(volume);
        }
    }

    public float GetMusicVolume()
    {
        return musicVolume;
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        PlayerPrefs.SetFloat("musicVolume", volume);
        PlayerPrefs.Save();

        // Update AudioManager volume
        if (audioManager != null)
        {
            audioManager.ChangeMusicVolume(volume);
        }
    }

    private void ChangeSoundVolume(float value)
    {
        SetSoundVolume(value);
    }

    private void ChangeMusicVolume(float value)
    {
        SetMusicVolume(value);
    }
}