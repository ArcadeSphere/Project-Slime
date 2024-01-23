using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
public class Health : MonoBehaviour
{
    [Header("Health Settings")]
    public float startingLives;
    [SerializeField] private Rigidbody2D rb;
    private Animator anim;

    [Header("Character Damage Settings")]
    public ParticleSystem hitParticle;
    [SerializeField] private AudioClip hurtSound;
    [SerializeField] private Flash flashing;

    [Header("Character Death Settings")]
    [SerializeField] private AudioClip deadSound;
    [SerializeField] private Behaviour[] behaviourComponents;
    private bool isDead;
    private float delaySoundInSeconds = 2.0f;

    [Header("Invincibility only for Player")]
    [SerializeField] private InvincibilityManager invincibilityController;


    public Health Instance { get; private set; }
    public float CurrentHealth { get; private set; }

    private void Awake()
    {
        CurrentHealth = startingLives;
        PlayerData.Instance.isPlayerDead = false;
        flashing = GetComponent<Flash>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        PlayerData.Instance.isPlayerDead = false;

        if (!gameObject.CompareTag("Player"))
        {
            invincibilityController = null;
        }
    }

    internal void SetCurrentHealth(float value)
    {
        CurrentHealth = value;
    }

    public void DamageCharacter(float damage)
    {
        if (IsInvulnerable()) return;

        SetCurrentHealth(Mathf.Clamp(CurrentHealth - damage, 0, startingLives));

        if (CurrentHealth > 0)
        {
            DamageSetting();
        }
        else
        {
            CharacterDeath();
        }
    }
    private bool IsInvulnerable()
    {
        return invincibilityController != null && invincibilityController.IsInvulnerable();
    }

    private void DamageSetting()
    {
        PlayHitParticleIfNeeded();
        flashing.FlashTime();
        AudioManager.instance.PlaySoundEffects(hurtSound);

        if (invincibilityController != null)
        {
            StartCoroutine(invincibilityController.BeInvincible());
        }
    }
    private void PlayHitParticleIfNeeded()
    {
        if (hitParticle && !gameObject.CompareTag("Player"))
        {
            hitParticle.Play();
        }
    }

    public void AddHealth(float value)
    {
        CurrentHealth = Mathf.Clamp(CurrentHealth + value, 0, startingLives);
    }

    public void CharacterDeath()
    {
        if (!isDead && rb != null)
        { 
            DisableComponents();
            flashing.FlashTime();
            PlayDeadAnimation();
            PlayerDeathLogic();
            PlayDeadSoundWithDelay();
            isDead = true;
            rb.sharedMaterial = null;
        }
    }
    private void PlayDeadAnimation()
    {
        anim.SetTrigger("dying");
    }
    private void PlayerDeathLogic()
    {
        if (gameObject.CompareTag("Player"))
        {
            PlayerData.Instance.isPlayerDead = true;
            anim.SetBool("grounded", true);
            CameraShake.Instance.ShakeCamera(3f, 0.2f);
        }
    }

    private void DisableComponents()
    {
        foreach (Behaviour component in behaviourComponents)
        {
            component.enabled = false;
        }
    }

    private void PlayDeadSoundWithDelay()
    {
        AudioManager.instance.PlaySoundEffects(deadSound);
    }

    public void DestroyThisCharacter()
    {
        Destroy(gameObject);
    }

    public void PlayHitParticleRight()
    {
        hitParticle.Play();
    }

    public void PlayHitParticleLeft()
    {
        hitParticle.transform.localScale = new Vector3(-1, 1, 1);
        hitParticle.Play();
    }
}