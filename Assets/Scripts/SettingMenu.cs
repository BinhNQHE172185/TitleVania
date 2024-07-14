using System.Collections;
using System.Collections.Generic;
using TMPro; // TextMeshPro namespace for text UI elements
using UnityEngine;
using UnityEngine.SceneManagement; // For managing scenes

public class SettingMenu : MonoBehaviour
{
    [SerializeField] private GameObject controlPanel;
    [SerializeField] private GameObject volumePanel;
    [SerializeField] int level;
    // Method to restart the game
    public void Restart()
    {
        Time.timeScale = 1f;
        string text = "Level " + level;
        SceneManager.LoadScene(text);
    }

    // Method to return to the main menu
    public void MainMenu()
    {
        // Load the scene named "menu" to go back to the main menu
        SceneManager.LoadScene("MainMenu");
    }
    public void OpenSettings()
    {
        // Pause the game
        Time.timeScale = 0f;
        gameObject.SetActive(true);
    }
    public void CloseSettings()
    {
        // Resume the game
        Time.timeScale = 1f;
        gameObject.SetActive(false);
        controlPanel.SetActive(false);
        volumePanel.SetActive(false);
    }
    public void OpenCloseSettings()
    {
        if (!gameObject.activeSelf)
        {
            // Pause the game
            Time.timeScale = 0f;
            gameObject.SetActive(true);
        }
        else  
        {
            // Resume the game
            Time.timeScale = 1f;
            gameObject.SetActive(false);
            controlPanel.SetActive(false);
            volumePanel.SetActive(false);   
        }
    }
}
