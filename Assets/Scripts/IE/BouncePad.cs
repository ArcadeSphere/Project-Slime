using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncePad : MonoBehaviour
{
    [SerializeField] private float bounceForce = 20f;
    [SerializeField] private AudioClip bounceSound;
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();   
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            anim.SetTrigger("Bounce");
            AudioManager.instance.PlaySoundEffects(bounceSound);
            PlayerMovement playerMovement = collision.gameObject.GetComponent<PlayerMovement>();

            // Check if the player is not currently dashing or jumping
            if (!playerMovement.IsDashing() && !playerMovement.IsJumping())
            {

                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);
            }
        }
    }
}


