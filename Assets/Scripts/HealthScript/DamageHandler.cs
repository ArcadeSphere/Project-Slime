using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageHandler : MonoBehaviour
{
    [SerializeField] private AudioClip hurtSound;

    [Header("Only for Player")]
    [SerializeField] private InvincibilityManager invincibilityController; 

    private Health health;

    private void Awake()
    {
        health = GetComponent<Health>();

      
        if (!gameObject.CompareTag("Player"))
        {
            invincibilityController = null;
        }
    }

    public void Damage(float damage)
    {
        if (invincibilityController != null && invincibilityController.IsInvulnerable()) return;

        health.SetCurrentHealth(Mathf.Clamp(health.CurrentHealth - damage, 0, health.startingLives));

        if (health.CurrentHealth > 0)
        {
            if (health.hitParticle && !gameObject.CompareTag("Player"))
            {
                health.hitParticle.Play();
            }

            health.flashing.FlashTime();
            AudioManager.instance.PlaySoundEffects(hurtSound);

            if (invincibilityController != null)
            {
                StartCoroutine(invincibilityController.BeInvincible());
            }
        }
        else
        {
            health.HandleDead();
        }
    }
}