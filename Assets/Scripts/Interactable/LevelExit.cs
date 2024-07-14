using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 1f;
    [SerializeField] string nextSceneName;
    void Start()
    {
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            StartCoroutine(LoadNextLevel());
        }
    }

    IEnumerator LoadNextLevel()
    {
        yield return new WaitForSecondsRealtime(levelLoadDelay);
        if (nextSceneName == "MainMenu")
        {
            FindObjectOfType<GameSession>().clearGameSession();
        }


        // Reset scene persist and load the next scene
        FindObjectOfType<ScenePersist>().ResetScenePersist();
        //SceneManager.LoadScene(nextSceneName);
        Initiate.Fade(nextSceneName, Color.black, 2.0f);

    }
}