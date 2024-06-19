using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UIGameOver : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] TextMeshProUGUI scoreText;
    ScenePersist scenePersist;
    GameSession gameSession;
     void Awake()
    {
        gameSession = FindObjectOfType<GameSession>();
    }

    void Start()
    {

        scoreText.text = "You Scored:\n" + gameSession.GetScore();
    }
}
