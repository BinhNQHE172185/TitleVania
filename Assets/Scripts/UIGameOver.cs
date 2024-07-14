using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UIGameOver : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] TextMeshProUGUI scoreText;

    void Start()
    {
        scoreText.text = "Final Score: " + GetScore();
    }
    public int GetScore()
    {
        string key = "Score";
        return PlayerPrefs.GetInt(key, 0);
    }
}
