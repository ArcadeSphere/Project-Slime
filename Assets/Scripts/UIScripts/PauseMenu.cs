using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
public class PauseMenu : MonoBehaviour
{
    private enum GameState
    {
        Playing,
        Paused,
        Options,
        Volume,
        Keybindings,
        Resolutions

    }

    private GameState currentState = GameState.Playing;

    [Header("Reference Settings")]
    [SerializeField] private PlayerCombat playerCombat;
    [SerializeField] private PlayerMovement playerMove;


    [Header("UI Settings")]
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject optionsMenuUI;
    [SerializeField] private GameObject volumeMenuUI;
    [SerializeField] private GameObject keybindingsMenuUI;
    [SerializeField] private GameObject resolutionUI;
    [SerializeField] private AudioClip buttonSound;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentState == GameState.Playing)
            {
                PauseGame();
            }
            else if (currentState == GameState.Paused)
            {
                ResumeGame();
            }
            else if (currentState == GameState.Options || currentState == GameState.Volume || currentState == GameState.Keybindings || currentState == GameState.Resolutions)
            {
                BackToPauseMenu();
            }
        }
    }

    void PauseGame()
    {
        currentState = GameState.Paused;
        Time.timeScale = 0f;
        pauseMenuUI.SetActive(true);
        playerCombat.enabled = false;
        playerMove.enabled = false;
    }

    public void ResumeGame()
    {
        currentState = GameState.Playing;
        AudioManager.instance.PlaySoundEffects(buttonSound);
        Time.timeScale = 1f;
        pauseMenuUI.SetActive(false);
        optionsMenuUI.SetActive(false);
        volumeMenuUI.SetActive(false);
        keybindingsMenuUI.SetActive(false);
        playerCombat.enabled = true;
        playerMove.enabled = true;
    }

    public void OpenOptions()
    {

        currentState = GameState.Options;
        AudioManager.instance.PlaySoundEffects(buttonSound);
        optionsMenuUI.SetActive(true);
        pauseMenuUI.SetActive(false);

    }

    public void OpenVolume()
    {
        currentState = GameState.Volume;
        AudioManager.instance.PlaySoundEffects(buttonSound);
        volumeMenuUI.SetActive(true);
        optionsMenuUI.SetActive(false);
    }

    public void OpenKeybindings()
    {
        currentState = GameState.Keybindings;
        AudioManager.instance.PlaySoundEffects(buttonSound);
        keybindingsMenuUI.SetActive(true);
        optionsMenuUI.SetActive(false);
    }
    public void OpenResolutions()
    {
        currentState = GameState.Resolutions;
        AudioManager.instance.PlaySoundEffects(buttonSound);
        resolutionUI.SetActive(true);
        optionsMenuUI.SetActive(false);
    }

    public void BackToPauseMenu()
    {
        currentState = GameState.Paused;
        AudioManager.instance.PlaySoundEffects(buttonSound);
        optionsMenuUI.SetActive(false);
        volumeMenuUI.SetActive(false);
        keybindingsMenuUI.SetActive(false);
        resolutionUI.SetActive(false);
        pauseMenuUI.SetActive(true);
       
    }

    public void RestartGame()
    {
        currentState = GameState.Playing;
        AudioManager.instance.PlaySoundEffects(buttonSound);
        Time.timeScale = 1f;
        Scene currentScene = SceneManager.GetActiveScene();
        CheckpointHandler.Instance.ResetCheckpointPrefValue();
        PlayerData.Instance.currentCheckpoint = null;
        SceneManager.LoadScene(currentScene.name);
    }

    public void RestartGameFromCheckPoint()
    {
        currentState = GameState.Playing;
        AudioManager.instance.PlaySoundEffects(buttonSound);
        Time.timeScale = 1f;
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void QuitGame()
    {
        AudioManager.instance.PlaySoundEffects(buttonSound);
        Application.Quit();
    }
}