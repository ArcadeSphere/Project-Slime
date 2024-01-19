using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathHandler : MonoBehaviour
{
    [SerializeField] private Behaviour[] behaviourComponents;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private AudioClip deadSound;

    private Health health;
    private Animator anim;
    private bool isDead;
    private float delaySoundInSeconds = 2.0f;

    private void Awake()
    {
        health = GetComponent<Health>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        PlayerData.Instance.isPlayerDead = false;
    }

    public void HandleDeath()
    {
        if (!isDead && rb != null)
        {
            if (gameObject.CompareTag("Player"))
            {
                PlayerData.Instance.isPlayerDead = true;
            }

            DisableComponents();
            health.flashing.FlashTime();

            if (gameObject.CompareTag("Player"))
            {
                anim.SetBool("grounded", true);
            }

            anim.SetTrigger("dying");
            Invoke("PlayDeadSoundWithDelay", delaySoundInSeconds);
            isDead = true;
            rb.sharedMaterial = null;
        }
    }

    //keeps all the behaviours here like patrol etc to stop them when death
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

    public void ShakeDeadCamera()
    {
        CameraShake.Instance.ShakeCamera(3f, 0.2f);
    }

    public void DestroyThisCharacter()
    {
        Destroy(gameObject);
    }
}