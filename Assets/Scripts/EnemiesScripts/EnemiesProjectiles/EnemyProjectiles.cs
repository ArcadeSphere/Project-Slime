using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
public class EnemyProjectiles : MonoBehaviour
{
    [Header("Projectile Settings")]
    public float destroyDelay = 2f;
    public GameObject explodeAnimation;
    [SerializeField] protected float enemyDamage;
    [SerializeField] private AudioClip explodeSound;

    private float speed;
    private Vector2 direction;

    private void Start()
    {
        Destroy(gameObject, destroyDelay);
    }

    private void Update()
    {
        MoveProjectile();
    }

    private void MoveProjectile()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    public void SetSpeed(float projectileSpeed)
    {
        speed = projectileSpeed;
    }

    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection;

        // Flip the projectile sprite based on the direction
        transform.localScale = new Vector3(direction.x > 0 ? -1 : 1, 1, 1);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (gameObject.CompareTag("Projectiles"))
        {
            HandleProjectileCollision(other);
        }
        else if (gameObject.CompareTag("Arrow"))
        {
            HandleArrowCollision(other);
        }
    }

    private void HandleProjectileCollision(Collider2D other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Player"))
        {
            AudioManager.instance.PlaySoundEffects(explodeSound);
            Instantiate(explodeAnimation, transform.position, Quaternion.identity);
            Destroy(gameObject);

            if (other.CompareTag("Enemy"))
            {
                other.GetComponent<Health>().TakeDamage(enemyDamage);
            }

            if (other.CompareTag("Player"))
            {
                HandlePlayerCollision(other);
            }
        }
    }

    private void HandleArrowCollision(Collider2D other)
    {
        if (other.CompareTag("Ground"))
        {
            DisableArrowPhysics();
        }

        if (other.CompareTag("Player"))
        {
            Debug.Log("Arrow hit the player");
            HandlePlayerCollision(other);
        }
    }

    private void HandlePlayerCollision(Collider2D playerCollider)
    {
        Vector3 hitDirection = playerCollider.transform.position - transform.position;

        if (hitDirection.x < 0)
        {
            playerCollider.GetComponent<Health>().Instance.PlayHitParticleRight();
        }
        else
        {
            playerCollider.GetComponent<Health>().Instance.PlayHitParticleLeft();
        }

        AudioManager.instance.PlaySoundEffects(explodeSound);
        SetSpeed(0f);

        DisableArrowPhysics();

        // Attach the arrow to the player
        transform.parent = playerCollider.transform;
    }

    private void DisableArrowPhysics()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.isKinematic = true;
        }

        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.enabled = false;
        }
    }
}