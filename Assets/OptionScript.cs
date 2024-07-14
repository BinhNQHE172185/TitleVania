using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OptionScript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI progressText;
    private int highScore;
    // Start is called before the first frame update
    void Start()
    {
        highScore = GetHighScore(0);
        UpdateProgressText(0);
    }

    public int GetHighScore(int level)
    {
        string key = "HighScore";
        return PlayerPrefs.GetInt(key, 0);
    }
    private void UpdateProgressText(int level)
    {
        String text = "";
        if (level > 0)
        {
            text = "Level - " + level + " - ";
        }
        highScore = GetHighScore(level);
        progressText.text = text + "High Sore: " + highScore;
    }
}
