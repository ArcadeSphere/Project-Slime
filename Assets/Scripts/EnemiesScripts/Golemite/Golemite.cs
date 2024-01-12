using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golemite : MonoBehaviour
{
    [Header("Golemite Parameters")]
    [SerializeField]
    private EnemyPatrol enemyPatrol;

    [SerializeField]
    private float dizzyCooldown = 2f;
    private PlayerDetector playerDetector;
    private Animator golemiteAnimator;

    private enum GolemState
    {
        Idle,
        Attack,
        Dizzy,
    }

    private GolemState currentState = GolemState.Idle;

    // debug
    [Header("Debug")]
    [SerializeField]
    private GameObject dt;
    private DebugText dbt;

    // debug


    private void Awake()
    {
        golemiteAnimator = this.GetComponent<Animator>();
        playerDetector = this.GetComponent<PlayerDetector>();
        // debug
        dbt = dt.GetComponent<DebugText>();
        // debug
    }

    private void Update()
    {
        // debug
        dbt.FollowParent(this.gameObject, this.GetComponent<SpriteRenderer>());
        dbt.SetText("Current State: ", currentState.ToString());
        // debug
        switch (currentState)
        {
            case GolemState.Idle:
                Idle();
                break;
            case GolemState.Attack:
                Attack();
                break;
            case GolemState.Dizzy:
                Dizzy();
                break;
        }
    }

    void Idle()
    {
        if (playerDetector.PlayerDetected && !enemyPatrol.onEdge)
        {
            currentState = GolemState.Attack;
            return;
        }
        golemiteAnimator.SetInteger("state", 0);
        enemyPatrol.GroundEnemyPatrol();
    }

    void Attack()
    {
        if (
            playerDetector.PlayerDetected && enemyPatrol.onEdge
            || !playerDetector.PlayerDetected && enemyPatrol.onEdge
        )
        {
            currentState = GolemState.Dizzy;
            return;
        }
        golemiteAnimator.SetInteger("state", 1);
        enemyPatrol.GroundEnemyPatrol();
    }

    void Dizzy()
    {
        golemiteAnimator.SetInteger("state", 0);
        StartCoroutine(ChangeStateAfterDelay(dizzyCooldown));
    }

    IEnumerator ChangeStateAfterDelay(float cooldown)
    {
        yield return new WaitForSeconds(cooldown);
        currentState = GolemState.Idle;
    }
}
