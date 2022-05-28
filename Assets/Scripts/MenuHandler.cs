using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuHandler : MonoBehaviour
{
    public static MenuHandler Instance { get; private set; }
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
        FindObjectOfType<PlayerController>().enabled = false;
        FindObjectOfType<PlayerSword>().enabled = false;
        Time.timeScale = 0; 
        pauseMenu.SetActive(true);
    }

    public void ClosePauseMenu()
    {
        _canPause = true;
        FindObjectOfType<PlayerController>().enabled = true;
        FindObjectOfType<PlayerSword>().enabled = true;
        Time.timeScale = 1; 
        pauseMenu.SetActive(false);
    }

    public void ReloadGame()
    {
        Time.timeScale = 1; 
        SceneManager.LoadScene(0);
    }

    public void CloseGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }

    public void OpenDeathMenu()
    {
        _canPause = false;
        gameOverMenu.SetActive(true);
        Time.timeScale = 0;
    }
}
