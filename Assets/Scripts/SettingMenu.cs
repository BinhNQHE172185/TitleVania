using System.Collections;
using System.Collections.Generic;
using TMPro; // TextMeshPro namespace for text UI elements
using UnityEngine;
using UnityEngine.SceneManagement; // For managing scenes

public class SettingMenu : MonoBehaviour
{
    private bool isOpenSettings = false;
    // Method to restart the game
    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Level 1");
    }

    // Method to return to the main menu
    public void MainMenu()
    {
        // Load the scene named "menu" to go back to the main menu
        SceneManager.LoadScene("MainMenu");
    }
    public void OpenCloseSettings()
    {
        if (!isOpenSettings)
        {
            // Pause the game
            Time.timeScale = 0f;
            gameObject.SetActive(true);
            isOpenSettings = true;
        }
        else
        {
            // Resume the game
            Time.timeScale = 1f;
            gameObject.SetActive(false);
            isOpenSettings = false;
        }
    }
}
