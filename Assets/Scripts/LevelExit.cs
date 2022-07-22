using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelExit : MonoBehaviour
{
    [Header("Invoke Level Delay")]
    [SerializeField] float reloadNextLevelTimeSec = 1f;
    [SerializeField] AudioClip exitSFX;
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
            AudioSource.PlayClipAtPoint(exitSFX, Camera.main.transform.position);
            Invoke("ReloadNextScene", reloadNextLevelTimeSec);
        }
    }
}
