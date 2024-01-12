using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
public class HealthPickup : MonoBehaviour
{
    [SerializeField] private float healingValue;
    [SerializeField] private AudioClip healSound;
       private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<Health>().AddHealth(healingValue);
            AudioManager.instance.PlaySoundEffects(healSound);
            Destroy(this.gameObject);
        }
    }
}