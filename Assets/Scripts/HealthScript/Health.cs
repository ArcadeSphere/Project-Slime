using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
public class Health : MonoBehaviour
{

    [Header("Health Settings")]
    public float startingLives;
    public ParticleSystem hitParticle;
  
    [Header("Reference Settings")]
    [SerializeField] private DeathHandler deathHandler;
    [SerializeField] private DamageHandler damageHandler;
    public Flash flashing;

    public Health Instance { get; private set; }
    public float CurrentHealth { get; private set; }
   

    private void Awake()
    {
        Instance = this;
        CurrentHealth = startingLives;
        PlayerData.Instance.isPlayerDead = false;
        damageHandler = GetComponent<DamageHandler>();
        deathHandler = GetComponent<DeathHandler>();
        flashing = GetComponent<Flash>();
    }
    internal void SetCurrentHealth(float value)
    {
        CurrentHealth = value;
    }
    public void TakeDamage(float damage)
    {
        damageHandler.Damage(damage);
    }
    public void HandleDead()
    {
        deathHandler.HandleDeath();
    }

    public void AddHealth(float value)
    {
        CurrentHealth = Mathf.Clamp(CurrentHealth + value, 0, startingLives);
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