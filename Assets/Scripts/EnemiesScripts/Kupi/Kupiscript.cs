using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kupiscript : PlayerDetector
{

    public Transform firePoint;
    public GameObject projectilePrefab;
    private Animator anim;
    public float projectileSpeed = 5f;
   [SerializeField] private AudioClip spitSound;
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (PlayerDetected)
        {
            OnPlayerIsIn();
        }
        else
        {
            OnPlayerIsOut();
        }
    }

    public void OnPlayerIsIn()
    {
        anim.SetBool("IsShooting", true);
       
    }

    public void OnPlayerIsOut()
    {
        anim.SetBool("IsShooting", false);
    }

    public void ShootPlayer()
    {
        AudioManager.instance.PlaySoundEffects(spitSound);
        Vector2 shootDirection = transform.localScale.x > 0 ? Vector2.right : Vector2.left;

        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        EnemyProjectiles projectileComponent = projectile.GetComponent<EnemyProjectiles>();

        if (projectileComponent != null)
        {
            projectileComponent.SetDirection(shootDirection);
            projectileComponent.SetSpeed(projectileSpeed);
        }
    }
}