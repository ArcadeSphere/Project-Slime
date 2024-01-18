using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteRenderActivations : MonoBehaviour
{
    private SpriteRenderer indicatorSpriteRenderer;
    private void Start()
    {
        indicatorSpriteRenderer = GetComponent<SpriteRenderer>();
        if (indicatorSpriteRenderer != null)
        {
            indicatorSpriteRenderer.enabled = false;
        }
    }

    public void EnableSpriteRenderer()
    {
        SetIndicatorSpriteRendererActive(true);
    }

    public void DisableSpriteRenderer()
    {
        SetIndicatorSpriteRendererActive(false);
    }

    private void SetIndicatorSpriteRendererActive(bool isActive)
    {
        if (indicatorSpriteRenderer != null)
        {
            indicatorSpriteRenderer.enabled = isActive;
        }
    }
}
