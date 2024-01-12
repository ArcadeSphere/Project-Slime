using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShaderSceneTransitionController : MonoBehaviour
{
    [SerializeField]
    private Sprite transitionImage;

    [SerializeField]
    private float maxProgress = 1.2f;

    [SerializeField]
    private float minProgress = -0.1f;

    [SerializeField]
    private float edgeSmoothing = 0.1f;

    [SerializeField]
    private Color transitionColor = Color.black;

    [SerializeField]
    private Image image;

    [SerializeField]
    private float transitionSpeed = 1f;
    private String progress = "_Progress";
    private String smoothing = "_EdgeSmoothing";
    private String color = "_Color";
    private Material material;
    private bool reveal = true;

    private void Awake()
    {
        image = image.GetComponent<Image>();
        material = image.material;
        material.SetFloat(progress, minProgress);
        material.SetFloat(smoothing, edgeSmoothing);
        material.SetColor(color, transitionColor);
        image.sprite = transitionImage;
        image.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (PlayerData.Instance.isPlayerDead)
        {
            reveal = false;
        }
        PlayTransition();
    }

    void PlayTransition()
    {
        if (reveal)
        {
            material.SetFloat(progress, ControlTransitionProgress(maxProgress));
        }
        else
        {
            material.SetFloat(progress, ControlTransitionProgress(minProgress));
            if (material.GetFloat(progress) == minProgress)
            {
                SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
            }
        }
    }

    float ControlTransitionProgress(float moveToValue)
    {
        return Mathf.MoveTowards(
            material.GetFloat(progress),
            moveToValue,
            transitionSpeed * Time.deltaTime
        );
    }
}
