using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
public class EnemyProjectiles : MonoBehaviour
{
    [Header("Projectiles Settings")]
    public float destroyDelay = 2f;
    public GameObject explodeanimation;
    [SerializeField] protected float enemyDamage;
    private float speed;
    private Vector2 direction;
    [SerializeField] private AudioClip explodeSound;
    private void Start()
    {
        Destroy(gameObject, destroyDelay);
    }

    private void Update()
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


        if (direction.x > 0)
        {

            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {

            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (gameObject.CompareTag("Projectiles"))
        {
            if (other.CompareTag("Enemy") || other.CompareTag("Player"))
            {
                AudioManager.instance.PlaySoundEffects(explodeSound);
                Instantiate(explodeanimation, transform.position, Quaternion.identity);
                Destroy(gameObject);

                if (other.CompareTag("Enemy"))
                {
                    other.GetComponent<Health>().TakeDamage(enemyDamage);
                    Destroy(gameObject);
                }

                if (other.CompareTag("Player"))
                {
                    Vector3 hitDirection = other.transform.position - transform.position;
                    if (hitDirection.x < 0)
                    {
                        other.GetComponent<Health>().Instance.PlayHitParticleRight();
                    }
                    else
                    {
                        other.GetComponent<Health>().Instance.PlayHitParticleLeft();
                    }
                    AudioManager.instance.PlaySoundEffects(explodeSound);
                    Instantiate(explodeanimation, transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }
            }
        }
        else if (gameObject.CompareTag("Arrow"))
        {
            if (other.CompareTag("Ground"))
            {
                Rigidbody2D rb = GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.velocity = Vector2.zero;
                    rb.isKinematic = true;
                }
                SetSpeed(0f);
                BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
                if (boxCollider != null)
                {
                    boxCollider.enabled = false;
                }
            }

            if (other.CompareTag("Player"))
          {
                Debug.Log("Arrow hit the player");
                Vector3 hitDirection = other.transform.position - transform.position;
                if (hitDirection.x < 0)
                {
                    other.GetComponent<Health>().Instance.PlayHitParticleRight();
                }
                else
                {
                    other.GetComponent<Health>().Instance.PlayHitParticleLeft();
                }

                AudioManager.instance.PlaySoundEffects(explodeSound);
                SetSpeed(0f);
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
                transform.parent = other.transform;

            }
        }
    }
}
