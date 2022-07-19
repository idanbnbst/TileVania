using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelExit : MonoBehaviour
{
    [Header("Invoke Level Delay")]
    [SerializeField] float reloadNextLevelTimeSec = 0.2f;
    void ReloadNextScene()
    {
        int currentBuildIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentBuildIndex + 1;

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
        if (other.tag == "Player")
        {
            Debug.Log("Level up");
            Invoke("ReloadNextScene", reloadNextLevelTimeSec);
        }
    }
}
