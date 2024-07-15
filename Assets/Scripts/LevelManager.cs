using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{

    public void LoadGame()
    {
        // Resume the game
        Time.timeScale = 1f;
        //SceneManager.LoadScene("Level 1");
        Initiate.Fade("Level 1", Color.black, 2.0f);
        PlayerPrefs.DeleteKey("Score");
    }
    public void LoadMainMenu()
    {
        // Load the scene named "menu" to go back to the main menu
        GameSession gameSession = FindObjectOfType<GameSession>();
        gameSession.clearGameSession();
        //SceneManager.LoadScene("MainMenu");
        Initiate.Fade("MainMenu", Color.black, 2.0f);

    }
    public void LoadGameOver()
    {
        //SceneManager.LoadScene("GameOver");
        Initiate.Fade("GameOver", Color.black, 2.0f);

    }
    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
