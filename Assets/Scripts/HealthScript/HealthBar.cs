using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthBar : MonoBehaviour
{
    [SerializeField] private Health healthscript; 
    [SerializeField] private Slider health_bar;


    void Start()
    {
        health_bar.maxValue = healthscript.CurrentHealth;
    }

    // Update is called once per frame
    void Update()
    {
        health_bar.value = healthscript.CurrentHealth;
    }
}
