using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
public class PlayerCombat : MonoBehaviour
{
    [Header("Player Combat")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private float attackRange;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float damageAmount;
    [SerializeField] private AudioClip attackSound;
    [SerializeField] private InputManager inputManager;
   
    public LayerMask enemiesLayer;
    private Animator anim;
    private float cooldownTimer = Mathf.Infinity;
    private PlayerMovement pm;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        pm = GetComponent<PlayerMovement>();

    }

    private void Update()
    {
        //player combat
        if (!pm.IsDashing() && inputManager.GetPlayerAttackInputDown() && cooldownTimer > attackCooldown)
            StartCoroutine(PerformAttack());


        cooldownTimer += Time.deltaTime;
    }
    private IEnumerator PerformAttack()
    {
        pm.EnablePlayerMovement(false);

        AudioManager.instance.PlaySoundEffects(attackSound);
        anim.SetTrigger("attack");
        cooldownTimer = 0;

       
        yield return new WaitForSeconds(0.3f);

      
        pm.EnablePlayerMovement(true);
    }
    public void DamageTarget() //damage target in range
    {
        
        
        Collider2D[] hittarget = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemiesLayer);

        foreach (Collider2D target in hittarget) {

            target.GetComponent<Health>().TakeDamage(damageAmount);
        }
    }

     void OnDrawGizmosSelected()
    {
       
        Gizmos.DrawWireSphere(attackPoint.position, attackRange); 
    }
}
