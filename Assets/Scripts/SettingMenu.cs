using System;
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
    [SerializeField] TextMeshProUGUI progressText;
    private int highScore;
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
            highScore=GetHighScore();
            UpdateProgressText();
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
    private void UpdateProgressText()
    {
        String text = "";
        if (level > 0)
        {
            text = "Level - " + level + " - ";
        }
        highScore = GetHighScore();
        progressText.text = text + "High Sore: " + highScore;
    }
    // Retrieve the high score
    public int GetHighScore()
    {
        string key = "HighScore";
        return PlayerPrefs.GetInt(key, 0);
    }
}
