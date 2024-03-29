using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
public class ExplodingFruit : MonoBehaviour
{
    [Header("Exploding Fruit")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] protected float enemyDamage;
    public float fruitgravity;
    public GameObject fruitimpact;
    [SerializeField] private AudioClip explodeSound;

    [Header("References Settings")]
    [SerializeField] private PlayerDetector playerDector;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerDector = GetComponent<PlayerDetector>();
        rb.gravityScale = 0;
    }
    
    private void Update()
    {
        if (playerDector.PlayerDetected)
        {
            Playernearfruit();
        }
       
    }
    public void Playernearfruit()
    {
     
        rb.gravityScale = fruitgravity;

    }
   

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            AudioManager.instance.PlaySoundEffects(explodeSound);
            collision.GetComponent<Health>().DamageCharacter(enemyDamage);
            Instantiate(fruitimpact, transform.position, Quaternion.identity);
            Destroy(gameObject);

        }
        AudioManager.instance.PlaySoundEffects(explodeSound);
        Instantiate(fruitimpact, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
}
