using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
public class MainMenu : MonoBehaviour
{
    [SerializeField] private PlayerCombat playerCombat;
    [SerializeField] private PlayerMovement playerMove;
    [SerializeField] private GameObject optionsMenuUI;
    [SerializeField] private GameObject volumeMenuUI;
    [SerializeField] private GameObject keybindingsMenuUI;
    [SerializeField] private GameObject DisplayMenuUI;
    private enum MainMenuState
    {
        Main,
        Options,
        Volume,
        Keybindings,
        Display
    }

    private MainMenuState currentState = MainMenuState.Main;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentState == MainMenuState.Options || currentState == MainMenuState.Volume || currentState == MainMenuState.Keybindings || currentState == MainMenuState.Display)
            {
                BackToMainMenu();
            }
        }
    }

    public void PlayGame()
    {
        playerCombat.enabled = true;
        playerMove.enabled = true;
        PlayerData.Instance.currentCheckpoint = null;
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }

    public void QuitTheGame()
    {
        Application.Quit();
    }

    public void OpenOptions()
    {
        currentState = MainMenuState.Options;
        optionsMenuUI.SetActive(true);
    }

    public void OpenVolume()
    {
        currentState = MainMenuState.Volume;
        volumeMenuUI.SetActive(true);
    }

    public void OpenKeybindings()
    {
        currentState = MainMenuState.Keybindings;
        keybindingsMenuUI.SetActive(true);
    }
    public void OpenDisplay()
    {
        currentState = MainMenuState.Display;
        DisplayMenuUI.SetActive(true);
    }
    public void BackToMainMenu()
    {
        currentState = MainMenuState.Main;
        optionsMenuUI.SetActive(false);
        volumeMenuUI.SetActive(false);
        keybindingsMenuUI.SetActive(false);
        DisplayMenuUI.SetActive(false);
    }

}
