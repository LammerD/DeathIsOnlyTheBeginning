using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuHandler : MonoBehaviour
{
    public static MenuHandler Instance { get; private set; }
    [SerializeField] private GameObject startMenu;
    [SerializeField] private GameObject gameOverMenu;
    [SerializeField] private GameObject pauseMenu;

    private bool _canPause= true;
    void Start()
    {
        Instance = this;
    }

    private void Update()
    {
        if(Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (_canPause)
            {
                OpenPauseMenu();
            }
        }
    }

    private void OpenPauseMenu()
    {
        _canPause = false;
        FindObjectOfType<PlayerHealth>().TogglePlayerControl(false);
        Time.timeScale = 0; 
        pauseMenu.SetActive(true);
    }

    public void ClosePauseMenu()
    {
        _canPause = true;
        FindObjectOfType<PlayerHealth>().TogglePlayerControl(true);
        Time.timeScale = 1; 
        pauseMenu.SetActive(false);
    }

    public void ReloadGame()
    {
        MusicHandler.instance.ResetGameMusic();
        Time.timeScale = 1; 
        SceneManager.LoadScene(0);
    }
    public void StartGame()
    {
        FindObjectOfType<PlayerHealth>().TogglePlayerControl(true);
        startMenu.SetActive(false);
        GameManager.Instance.StartGame();
    }

    public void CloseGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #elif (UNITY_WEBGL)
        Application.OpenURL("https://domenixius.itch.io/growth-circle");
        #endif
    }

    public void OpenDeathMenu()
    {
        _canPause = false;
        gameOverMenu.SetActive(true);
        Time.timeScale = 0;
    }

    public void OpenStartMenu()
    {
        FindObjectOfType<PlayerHealth>().TogglePlayerControl(false);
        _canPause = false;
        startMenu.SetActive(true);
    }
}
