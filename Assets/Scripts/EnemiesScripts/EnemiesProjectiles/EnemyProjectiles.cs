using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
public class EnemyProjectiles : MonoBehaviour
{

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
        AudioManager.instance.PlaySoundEffects(explodeSound);
        Instantiate(explodeanimation, transform.position, Quaternion.identity);
        Destroy(gameObject);

        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Health>().TakeDamage(enemyDamage);
            AudioManager.instance.PlaySoundEffects(explodeSound);
            Instantiate(explodeanimation, transform.position, Quaternion.identity);
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
