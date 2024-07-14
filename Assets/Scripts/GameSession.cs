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
    [SerializeField] TextMeshProUGUI progressText;
    private int highScore;
    LevelManager levelManager;
    public int GetScore()
    {
        return score;
    }
    void Awake()
    {
        int numGameSessions = FindObjectsOfType<GameSession>().Length;
        levelManager = FindObjectOfType<LevelManager>();
        UpdateProgressText();
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
            ResetGameSession();
        }
    }

    public void AddToScore(int pointsToAdd)
    {
        score += pointsToAdd;
        SaveHighScore(score);
        UpdateProgressText();
        scoreText.text = "Score: " + score;
    }

    void TakeLife()
    {
        playerLives--;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
        livesText.text = "Lives: " + playerLives;
    }
    // Call this method to save the high score
    void SaveHighScore(int score)
    {
        // Check if the new score is higher than the current high score
        if (score > PlayerPrefs.GetInt("HighScore", 0))
        {
            PlayerPrefs.SetInt("HighScore", score);
            PlayerPrefs.Save(); // Save the changes to PlayerPrefs
            Debug.Log("New high score saved: " + score);
        }
    }
    // Retrieve the high score
    public int GetHighScore()
    {
        return PlayerPrefs.GetInt("HighScore", 0);
    }
    private void UpdateProgressText()
    {
        highScore = GetHighScore();
        progressText.text = "Level 1 - High Sore: " + highScore;
    }
    void ResetGameSession()
    {
        SceneManager.LoadScene(0);
        levelManager.LoadGameOver();
        Destroy(gameObject);
    }
    public void clearGameSession()
    {
        Destroy(gameObject);
    }
}
