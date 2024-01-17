using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GobiHood : MonoBehaviour
{
    [Header("Reference settings")]
    [SerializeField] private PlayerDetector playerDetector;
    [SerializeField] private EnemyController characterFlip;
    [SerializeField] private EnemyPatrol enemyPatrol;
    private Animator anim;
  private enum GobiHoodStates
    {
        Patrol,
        Detect,
        Shoot,
        ShootCooldown
    }

    private GobiHoodStates currentStates = GobiHoodStates.Patrol;
    private void Awake()
    {
        characterFlip = GetComponent<EnemyController>();
        playerDetector = GetComponent<PlayerDetector>();
        anim = GetComponent<Animator>();

    }

    // Update is called once per frame
    private void Update()
    {
        switch (currentStates)
        {
            case GobiHoodStates.Patrol:
                Patrol();
                break;

            case GobiHoodStates.Detect:
                DetectionDelay();
                break;

            case GobiHoodStates.Shoot:
                ShootAttack();
                break;

            case GobiHoodStates.ShootCooldown:
                CooldownDuringShots();
                break;
        }
    }

    private void Patrol()
    {
        
        enemyPatrol.GroundEnemyPatrol();
        anim.SetFloat("moveSpeed", 1f);
    }
    private void DetectionDelay()
    {

    }
    private void ShootAttack()
    {

    }
    private void CooldownDuringShots()
    {

    }

}
