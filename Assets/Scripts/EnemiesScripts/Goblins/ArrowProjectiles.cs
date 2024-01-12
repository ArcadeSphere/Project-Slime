using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
public class ArrowProjectiles : MonoBehaviour
{
    public float destroyArrowDelay = 2f;
    public GameObject explodeArrowanimation;
    [SerializeField] protected float enemyDamage;
    private float speed;
    private Vector2 direction;

    private void Start()
    {
        Destroy(gameObject, destroyArrowDelay);
    }

    private void Update()
    {
        // Use the 'direction' variable for movement
        transform.Translate(direction * speed * Time.deltaTime);
    }

    public void SetSpeed(float projectileSpeed)
    {
        speed = projectileSpeed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Instantiate(explodeArrowanimation, transform.position, Quaternion.identity);
        Destroy(gameObject);

        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Health>().TakeDamage(enemyDamage);
            Instantiate(explodeArrowanimation, transform.position, Quaternion.identity);
            Destroy(gameObject);

        }
        if (other.CompareTag("Player"))
        {
            Instantiate(explodeArrowanimation, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    public void SetProjectileDirection(Vector2 newDirection)
    {
        direction = newDirection;

        // Flip the projectile sprite based on the direction
        if (direction.x < 0)
        {
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }
}