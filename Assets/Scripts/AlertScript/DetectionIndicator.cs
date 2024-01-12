using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionIndicator : MonoBehaviour
{
    //this script purpose is only to display the ! 
    [Header("Alert Settings")]
    [SerializeField] private GameObject indicatorObject;
    private bool hasPlayedSound = false;
    [SerializeField] private AudioClip alertSound;


    private void Start()
    {
        if (indicatorObject != null)
        {
            indicatorObject.SetActive(false);
        }
    }

    public void ActivateAlert()
    {
        SetIndicatorActive(true);
        PlayAlertSound();
    }


    public void DeactivateAlert()
    {
        SetIndicatorActive(false);
        hasPlayedSound = false;
    }

    private void SetIndicatorActive(bool isActive)
    {
        if (indicatorObject != null)
        {
            indicatorObject.SetActive(isActive);
        }
    }

    private void PlayAlertSound()
    {
        if (indicatorObject != null && !hasPlayedSound)
        {
            AudioManager.instance.PlaySoundEffects(alertSound);
            hasPlayedSound = true;
        }
    }
}