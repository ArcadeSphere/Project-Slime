using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.PackageManager;
using UnityEngine;

public class DebugText : MonoBehaviour
{
    private Enemy enemy;

    [SerializeField]
    private GameObject parent;

    [SerializeField]
    private float yOffset = 0;

    [SerializeField]
    private bool showState = true;
    private SpriteRenderer sr;
    private RectTransform rt;

    private TextMeshPro tmp;

    private const float OffsetMultiplier = 0.4f;

    private void Start()
    {
        rt = GetComponent<RectTransform>();
        sr = parent.GetComponent<SpriteRenderer>();
        tmp = GetComponent<TextMeshPro>();
        enemy = parent.GetComponent<Enemy>();
    }

    private void Update()
    {
        FollowParent();

        ShowState();
    }

    public void FollowParent()
    {
        rt.position =
            parent.transform.position
            + new Vector3(
                0,
                (sr.bounds.size.y * 0.5f) + Mathf.Floor(yOffset) * OffsetMultiplier,
                0
            );
    }

    public void ShowState()
    {
        if (showState)
            tmp.text = "Current State: " + enemy.StateMachine.CurrentEnemyState.ToString();
        else
            tmp.text = "";
    }
}
