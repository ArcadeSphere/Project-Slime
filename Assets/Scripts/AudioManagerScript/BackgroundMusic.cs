using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    [SerializeField] private AudioClip backGround;

    //add your background music
    void Start()
    {
        AudioManager.instance.PlayBackground(backGround);
    }

  
}
