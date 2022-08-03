using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelExit : MonoBehaviour
{
    [Header("Invoke Level Delay")]
    [SerializeField] float reloadNextLevelTime = 1f;
    [SerializeField] float restartGameTime = 2f;
    [Header("SFX")]
    [SerializeField] AudioClip exitSFX;
    [SerializeField] AudioClip winSFX;
    bool hasExit = false;
    void ReloadNextScene()
    {
        int currentBuildIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentBuildIndex + 1;

        // Game Completed
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            FindObjectOfType<GameSession>().ResetGameSession();
            return;
        }
        FindObjectOfType<ScenePersist>().ResetScenePersist();
        SceneManager.LoadScene(nextSceneIndex);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && !hasExit)
        {
            hasExit = true;

            int currentBuildIndex = SceneManager.GetActiveScene().buildIndex;
            int nextSceneIndex = currentBuildIndex + 1;
            if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
            {
                AudioSource.PlayClipAtPoint(winSFX, Camera.main.transform.position);
                FindObjectOfType<GameSession>().DisplayEndScreen();
                Invoke("ReloadNextScene", restartGameTime);
            }
            else
            {
                AudioSource.PlayClipAtPoint(exitSFX, Camera.main.transform.position);
                Invoke("ReloadNextScene", reloadNextLevelTime);
            }
        }
    }
    public bool HasExit()
    {
        return hasExit;
    }
}