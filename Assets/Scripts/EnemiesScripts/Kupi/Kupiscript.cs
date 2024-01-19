using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kupiscript : MonoBehaviour
{
    [Header("Kupis Settings")]
    public Transform firePoint;
    public GameObject projectilePrefab;
    public float projectileSpeed = 5f;
    [SerializeField] private AudioClip spitSound;

    [Header("References Settings")]
    [SerializeField] private PlayerDetector playerDetector;

    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerDetector = GetComponent<PlayerDetector>();
    }

    private void Update()
    {
        UpdateAnimatorState();
    }

    private void UpdateAnimatorState()
    {
        int newState = playerDetector.PlayerDetected ? 1 : 0;
        SetAnimatorState(newState);
    }

    private void SetAnimatorState(int state)
    {
        anim.SetInteger("state", state);
    }

    public void ShootPlayer()
    {
        PlaySpitSound();
        Vector2 shootDirection = GetShootDirection();

        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        ConfigureProjectile(projectile, shootDirection);
    }

    private void PlaySpitSound()
    {
        AudioManager.instance.PlaySoundEffects(spitSound);
    }

    private Vector2 GetShootDirection()
    {
        return transform.localScale.x > 0 ? Vector2.right : Vector2.left;
    }

    private void ConfigureProjectile(GameObject projectile, Vector2 shootDirection)
    {
        EnemyProjectiles projectileComponent = projectile.GetComponent<EnemyProjectiles>();

        if (projectileComponent != null)
        {
            projectileComponent.SetDirection(shootDirection);
            projectileComponent.SetSpeed(projectileSpeed);
        }
    }
}