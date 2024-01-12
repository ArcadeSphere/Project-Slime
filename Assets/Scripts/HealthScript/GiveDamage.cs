using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveDamage : MonoBehaviour
{
    //Made this script because in the takingDamage script will get many errors use this script for taking damage from enemies attack..Use Animation events
    [Header("Attack Detection Settings")]
    [SerializeField] private Transform attackDetectionZoneOrigin;
    [SerializeField] private float attackDetectionRange;
    [SerializeField] private float attackDamage;
    [SerializeField] private LayerMask playerLayer;
    public void InflictDamageOnPlayer()
    {

        Collider2D[] hitTargets = Physics2D.OverlapCircleAll(attackDetectionZoneOrigin.position, attackDetectionRange, playerLayer);

        foreach (Collider2D target in hitTargets)
        {
            target.GetComponent<Health>().TakeDamage(attackDamage);
        }

    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackDetectionZoneOrigin.position, attackDetectionRange);
    }
}
