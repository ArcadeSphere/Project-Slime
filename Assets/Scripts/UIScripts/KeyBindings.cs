using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Serialization;
using System;
public class KeyBindings : MonoBehaviour
{
    public InputManager inputManager;

   
    public Button jumpButton;
    public Button moveLeftButton;
    public Button moveRightButton;
    public Button dashButton;
    public Button platformDisableButton;
    public Button playerAttackButton;
    public Button playerInteractButton;

    public Button restoreDefaultsButton;

    private Button currentButton;

    void Start()
    {
        // Add listener here
        AddButtonListener(jumpButton);
        AddButtonListener(moveLeftButton);
        AddButtonListener(moveRightButton);
        AddButtonListener(dashButton);
        AddButtonListener(platformDisableButton);
        AddButtonListener(playerAttackButton);
        AddButtonListener(playerInteractButton);

        LoadKeyBindingsToButtons();
    }

    void Update()
    {
        if (currentButton != null)
        {
            // Check for any key press
            foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(keyCode) && keyCode != KeyCode.None)
                {
                    SetButtonKey(currentButton, keyCode);
                    currentButton = null;
                }
            }
        }
    }

    private void AddButtonListener(Button button)
    {
        button.onClick.AddListener(() => SetCurrentButton(button));
    }

    private void SaveKeyBindings()
    {
        inputManager.jumpKey = jumpButton.GetComponentInChildren<Text>().text;
        inputManager.moveLeftKey = moveLeftButton.GetComponentInChildren<Text>().text;
        inputManager.moveRightKey = moveRightButton.GetComponentInChildren<Text>().text;
        inputManager.dashKey = dashButton.GetComponentInChildren<Text>().text;
        inputManager.platformDisableKey = platformDisableButton.GetComponentInChildren<Text>().text;
        inputManager.playerAttackKey = playerAttackButton.GetComponentInChildren<Text>().text;
        inputManager.playerInteractKey = playerInteractButton.GetComponentInChildren<Text>().text;
        inputManager.SaveKeybindings();
    }

    public void RestoreDefaults()
    {
        inputManager.LoadDefaultKeybindings();
        LoadKeyBindingsToButtons();
        SaveKeyBindings();
    }

    private void SetCurrentButton(Button button)
    {
        currentButton = button;
        currentButton.GetComponentInChildren<Text>().text = "Waiting";
    }

    // Check if the new key is already assigned to another button
    private void SetButtonKey(Button button, KeyCode newKeyCode)
    {
        string buttonText = newKeyCode.ToString();

        if (IsKeyAssignedToButton(buttonText))
        {
            
            KeyCode newKey = FindUnusedKey();
            button.GetComponentInChildren<Text>().text = newKey.ToString();
            SaveKeyBindings();
        }
        else
        {
            button.GetComponentInChildren<Text>().text = buttonText;
            SaveKeyBindings();
        }
    }

    private bool IsKeyAssignedToButton(string key)
    {
        return key == jumpButton.GetComponentInChildren<Text>().text ||
               key == moveLeftButton.GetComponentInChildren<Text>().text ||
               key == moveRightButton.GetComponentInChildren<Text>().text ||
               key == dashButton.GetComponentInChildren<Text>().text ||
               key == platformDisableButton.GetComponentInChildren<Text>().text ||
               key == playerAttackButton.GetComponentInChildren<Text>().text ||
               key == playerInteractButton.GetComponentInChildren<Text>().text;
    }
    // Find the unused key
    private KeyCode FindUnusedKey()
    {
      
        foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
        {
            if (!IsKeyAssignedToButton(keyCode.ToString()))
            {
                return keyCode;
            }
        }

        
        return KeyCode.Space;
    }

    // Load existing key bindings to buttons
    private void LoadKeyBindingsToButtons()
    {
        
        jumpButton.GetComponentInChildren<Text>().text = inputManager.jumpKey;
        moveLeftButton.GetComponentInChildren<Text>().text = inputManager.moveLeftKey;
        moveRightButton.GetComponentInChildren<Text>().text = inputManager.moveRightKey;
        dashButton.GetComponentInChildren<Text>().text = inputManager.dashKey;
        platformDisableButton.GetComponentInChildren<Text>().text = inputManager.platformDisableKey;
        playerAttackButton.GetComponentInChildren<Text>().text = inputManager.playerAttackKey;
        playerInteractButton.GetComponentInChildren<Text>().text = inputManager.playerInteractKey;
    }
}