using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class InputManager : MonoBehaviour
{
    public static InputManager instance;

    //Define your input here as strings
    public string jumpKey = "Space";
    public string moveLeftKey = "A";
    public string moveRightKey = "D";
    public string dashKey = "LeftShift";
    public string platformDisableKey = "S";
    public string playerAttackKey = "J";
    public string playerInteractKey = "E";
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        LoadKeybindings();
    }

    public bool GetJumpInput()
    {
        return Input.GetKeyDown(GetKeyCode(jumpKey));
    }

    public bool GetJumpInputUp()
    {
        return Input.GetKeyUp(GetKeyCode(jumpKey));
    }

    public float GetHorizontalInput()
    {
        float horizontalInput = 0f;

        if (Input.GetKey(GetKeyCode(moveLeftKey)))
        {
            horizontalInput -= 1f;
        }

        if (Input.GetKey(GetKeyCode(moveRightKey)))
        {
            horizontalInput += 1f;
        }

        return horizontalInput;
    }

    public bool GetDashInputDown()
    {
        return Input.GetKeyDown(GetKeyCode(dashKey));
    }


    public bool GetPlatformDisableInputDown()
    {
        return Input.GetKeyDown(GetKeyCode(platformDisableKey));
    }

    public bool GetPlayerAttackInputDown()
    {
        return Input.GetKeyDown(GetKeyCode(playerAttackKey));
    }
    public bool GetPlayerInteractInputDown()
    {
        return Input.GetKeyDown(GetKeyCode(playerInteractKey));
    }
    public bool GetArrowInput(string arrowKeyName)
    {
        return Input.GetKey(GetKeyCode(arrowKeyName));
    }

    // Save keybindings to PlayerPrefs
    public void SaveKeybindings()
    {
        PlayerPrefs.SetString("JumpKey", jumpKey);
        PlayerPrefs.SetString("MoveLeftKey", moveLeftKey);
        PlayerPrefs.SetString("MoveRightKey", moveRightKey);
        PlayerPrefs.SetString("DashKey", dashKey);
        PlayerPrefs.SetString("PlatformDisableKey", platformDisableKey);
        PlayerPrefs.SetString("PlayerAttackKey", playerAttackKey);
        PlayerPrefs.SetString("PlayerInteractKey", playerInteractKey);
    }

    // Load keybindings
    public void LoadKeybindings()
    {
        jumpKey = PlayerPrefs.GetString("JumpKey", "Space");
        moveLeftKey = PlayerPrefs.GetString("MoveLeftKey", "A");
        moveRightKey = PlayerPrefs.GetString("MoveRightKey", "D");
        dashKey = PlayerPrefs.GetString("DashKey", "LeftShift");
        platformDisableKey = PlayerPrefs.GetString("PlatformDisableKey", "S");
        playerAttackKey = PlayerPrefs.GetString("PlayerAttackKey", "J");
        playerInteractKey = PlayerPrefs.GetString("PlayerInteractKey", "E");
    }
    // Load the defualt keybindings
    public void LoadDefaultKeybindings()
    {
        jumpKey = "Space";
        moveLeftKey = "A";
        moveRightKey = "D";
        dashKey = "LeftShift";
        platformDisableKey = "S";
        playerAttackKey = "J";
        playerInteractKey = "E";
    }


    // Method to convert string to KeyCode
   public KeyCode GetKeyCode(string keyName)
    {
        if (keyName.StartsWith("Arrow") || keyName.StartsWith("Alpha"))
        {
            return (KeyCode)System.Enum.Parse(typeof(KeyCode), keyName);
        }

        try
        {
            return (KeyCode)System.Enum.Parse(typeof(KeyCode), keyName, true);
        }
        catch (ArgumentException)
        {
            Debug.LogError($"Invalid key name: {keyName}");
            return KeyCode.None;
        }
    }
}