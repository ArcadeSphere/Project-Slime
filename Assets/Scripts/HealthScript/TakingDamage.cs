using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TakingDamage : MonoBehaviour
{
    [Header("OnTriggerDamage Settings")]
    [SerializeField]
    protected float damage;

    [Header("Required if not using is trigger")]
    [SerializeField]
    private PlayerDetector playerDetector;

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Health>().TakeDamage(damage);
            CameraShake.Instance.ShakeCamera(2f, 0.2f);
        }
    }

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && playerDetector.PlayerDetected)
        {
            collision.gameObject.GetComponent<Health>().TakeDamage(damage);
            CameraShake.Instance.ShakeCamera(2f, 0.2f);
        }
    }
}
