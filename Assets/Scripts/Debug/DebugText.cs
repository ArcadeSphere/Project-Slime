using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.PackageManager;
using UnityEngine;

public class DebugText : MonoBehaviour
{
    private RectTransform rt;

    [HideInInspector]
    private TextMeshPro tmp;
    private const float OffsetMultiplier = 0.4f;

    private void Start()
    {
        rt = GetComponent<RectTransform>();
        tmp = GetComponent<TextMeshPro>();
    }

    public void FollowParent(GameObject parent, SpriteRenderer sr, float yOffset = 0)
    {
        rt.position =
            parent.transform.position
            + new Vector3(
                0,
                (sr.bounds.size.y * 0.5f) + Mathf.Floor(yOffset) * OffsetMultiplier,
                0
            );
    }

    public void SetText(String infoName, String value)
    {
        tmp.text = infoName + value;
    }
}
