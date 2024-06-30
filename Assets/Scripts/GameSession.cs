using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Diagnostics;

public class GameSession : MonoBehaviour
{
    [SerializeField] int playerLives = 3;
    [SerializeField] int score = 0;
    [SerializeField] TextMeshProUGUI livesText;
    [SerializeField] TextMeshProUGUI scoreText;
    LevelManager levelManager;
    public int GetScore()
    {
        return score;
    }
    void Awake()
    {
        int numGameSessions = FindObjectsOfType<GameSession>().Length;
        levelManager = FindObjectOfType<LevelManager>();
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
        scoreText.text = "Score: " + score;
    }

    void TakeLife()
    {
        playerLives--;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
        livesText.text = "Lives: " + playerLives;
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
