using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvincibilityManager : MonoBehaviour
{
    private bool invulnerable;

    [SerializeField]
    private float Framesduration;

    [SerializeField]
    private int numberOfFlashes;
    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public bool IsInvulnerable()
    {
        return invulnerable;
    }

    public IEnumerator BeInvincible()
    {
        invulnerable = true;
        Physics2D.IgnoreLayerCollision(10, 11, true);

        for (int i = 0; i < numberOfFlashes; i++)
        {
            sr.color = new Color(0, 1, 0, 0.5f);
            yield return new WaitForSeconds(Framesduration / (numberOfFlashes * 2));
            sr.color = Color.white;
            yield return new WaitForSeconds(Framesduration / (numberOfFlashes * 2));
        }

        Physics2D.IgnoreLayerCollision(10, 11, false);
        invulnerable = false;
    }
}
