using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class GameSession : MonoBehaviour
{
    [SerializeField] int playerLives = 3;
    [SerializeField] int score = 0;
    [SerializeField] TextMeshProUGUI livesText;
    [SerializeField] TextMeshProUGUI scoreText;
/*    [SerializeField] TextMeshProUGUI progressText;*/
    [SerializeField] int level = 1;
/*    private int highScore;*/
    LevelManager levelManager;
    public int GetScore()
    {
        return score;
    }
    void Awake()
    {
        int numGameSessions = FindObjectsOfType<GameSession>().Length;
        levelManager = FindObjectOfType<LevelManager>();
/*        UpdateProgressText(level);*/
        if (numGameSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        livesText.text = "Lives: " + playerLives;
        scoreText.text = "Score: " + score;
    }

    public void ProcessPlayerDeath()
    {
        if (playerLives > 1)
        {
            TakeLife();
        }
        else
        {
            Debug.Log(playerLives + "Reset");
            ResetGameSession();
        }
    }

    public void AddToScore(int pointsToAdd)
    {
        score += pointsToAdd;
        SaveHighScore(level, score);
        SaveScore(score);
/*        UpdateProgressText(level);*/
        scoreText.text = "Score: " + score;
    }

    void TakeLife()
    {
        Debug.Log(playerLives);
        playerLives--;
        Debug.Log(playerLives);
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        //SceneManager.LoadScene(currentSceneIndex);
        Initiate.Fade(SceneManager.GetActiveScene().name, Color.black, 2.0f);

        livesText.text = "Lives: " + playerLives;
    }
    void SaveScore(int score)
    {
        String key = "Score";
        PlayerPrefs.SetInt(key, score);
        PlayerPrefs.Save(); // Save the changes to PlayerPrefs
    }
    // Call this method to save the high score
    void SaveHighScore(int level, int score)
    {
        String key = "HighScore";
        // Check if the new score is higher than the current high score
        if (score > PlayerPrefs.GetInt(key, 0))
        {
            PlayerPrefs.SetInt(key, score);
            PlayerPrefs.Save(); // Save the changes to PlayerPrefs
            Debug.Log("New high score saved: " + score);
        }
    }
/*    // Retrieve the high score
    public int GetHighScore(int level)
    {
        string key = "HighScore";
        return PlayerPrefs.GetInt(key, 0);
    }*/
/*    private void UpdateProgressText(int level)
    {
        String text = "";
        if (level > 0)
        {
            text = "Level - " + level + " - ";
        }
        highScore = GetHighScore(level);
        progressText.text = text + "High Sore: " + highScore;
    }*/
    public void ResetGameSession()
    {
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }
    public void clearGameSession()
    {
        Destroy(gameObject);
    }
}
