using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 1f;
    [SerializeField] string nextSceneName;
    List<string> levelNames = new List<string>(); // List of level names
    void Start()
    {
        /*// Initialize the dictionary
        levelNameMap = new Dictionary<string, string>
        {
            { "Level 1", "MainMenu" }
            // Add other levels as needed
        };*/
        levelNames = new List<string>() {
         "Level 1", "Level 2", "Level 3", "Hidden 1", "Hidden 2", "Hidden 3", "MainMenu"
        };
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
        // Check if the next scene name is in the levelNames list
        if (!levelNames.Contains(nextSceneName))
        {
            Debug.LogError("Next scene name specified in nextMap is not a valid level name!");
            yield break;
        }
        if (nextSceneName == "MainMenu")
        {
            FindObjectOfType<GameSession>().clearGameSession();
        }


        // Reset scene persist and load the next scene
        FindObjectOfType<ScenePersist>().ResetScenePersist();
        SceneManager.LoadScene(nextSceneName);
    }
}