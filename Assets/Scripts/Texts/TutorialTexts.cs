using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TutorialTexts : MonoBehaviour
{
  

    [SerializeField] private InputManager inputManager;
    [SerializeField] private TextMeshPro textMeshPro;
    [TextArea] public string customMessage;
 
    [Header("GIVE A FUNCTION")]
    public DisplayFunction displayFunction = DisplayFunction.MoveKeys;
    public enum DisplayFunction
    {
        MoveKeys,
        PlayerAttackButton,
        Jump,
        Dash,
        Messages


    }
    private void Start()
    {
        if (inputManager == null)
        {
            Debug.LogError("InputManager reference is missing!");
        }

        if (textMeshPro == null)
        {
            Debug.LogError("TextMeshPro reference is missing!");
        }
    }

    private void Update()
    {
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        switch (displayFunction)
        {
            case DisplayFunction.MoveKeys:
                DisplayMoveKeys();
                break;
            case DisplayFunction.PlayerAttackButton:
                DisplayPlayerAttackButton();
                break;
            case DisplayFunction.Jump:
                DisplayJump();
                break;
            case DisplayFunction.Dash:
                DisplayDash();
                break;
            case DisplayFunction.Messages:
                DisplayMessage(customMessage);
                break;
            default:
                Debug.LogError("Invalid DisplayFunction selected.");
                break;
        }
    }

    private void DisplayMoveKeys()
    {
        if (inputManager != null && textMeshPro != null)
        {
            string moveLeftText = $"Move Left:  <color=green><uppercase> {inputManager.moveLeftKey}</uppercase></color>";
            string moveRightText = $"Move Right: <color=green> <uppercase>{inputManager.moveRightKey}</uppercase>";

            textMeshPro.text = $"{moveLeftText}\n{moveRightText}";
        }
    }

    private void DisplayPlayerAttackButton()
    {
        if (inputManager != null && textMeshPro != null)
        {
            string playerAttackText = $"Attack:<color=green> <uppercase> {inputManager.playerAttackKey}</uppercase> ";

            textMeshPro.text = playerAttackText;
        }
    }

    private void DisplayJump()
    {
        if (inputManager != null && textMeshPro != null)
        {
            string jumpText = $"Jump: <color=green><uppercase> {inputManager.jumpKey}</uppercase>";

            textMeshPro.text = jumpText;
        }
    }

    private void DisplayDash()
    {
        if (inputManager != null && textMeshPro != null)
        {
            string dashText = $"Dash: <color=green><uppercase> {inputManager.dashKey} </uppercase> ";

            textMeshPro.text = dashText;
        }
    }
    private void DisplayMessage(string message)
    {
        if (textMeshPro != null)
        {
            textMeshPro.text = message;
        }
    }
}