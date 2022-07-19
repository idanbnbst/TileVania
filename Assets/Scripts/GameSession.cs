using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    [SerializeField] int numOfLives = 3;
    [SerializeField] int score = 0;
    [SerializeField] int scoreLose = 200;
    [SerializeField] int magazine = 10;
    [SerializeField] float reloadSceneTimeSec = 1.5f;
    [SerializeField] TextMeshProUGUI livesLabel;
    [SerializeField] TextMeshProUGUI scoreLabel;
    [SerializeField] TextMeshProUGUI magazineLabel;
    void Awake()
    {
        int numGameSessions = FindObjectsOfType<GameSession>().Length;
        if (numGameSessions > 1)
            Destroy(gameObject);
        else
            DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        livesLabel.text = numOfLives.ToString();
        scoreLabel.text = score.ToString();
        magazineLabel.text = magazine.ToString();
    }
    public void ProcessPlayerDeath()
    {
        if (numOfLives > 1)
            Invoke("DecreaseLife", reloadSceneTimeSec);
        else
            Invoke("ResetGameSession", reloadSceneTimeSec);
    }

    public void IncreaseLife()
    {
        numOfLives++;
        livesLabel.text = numOfLives.ToString();
    }
    void DecreaseLife()
    {
        numOfLives--;
        score -= scoreLose;
        if (score < 0)
            score = 0;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
        livesLabel.text = numOfLives.ToString();
        scoreLabel.text = score.ToString();
    }

    public int GetScore()
    {
        return score;
    }
    public void SetScore(int points)
    {
        score += points;
        scoreLabel.text = score.ToString();
    }

    public int GetMagazineRounds()
    {
        return magazine;
    }
    public void DecreaseMagazineRounds()
    {
        magazine--;
        magazineLabel.text = magazine.ToString();
    }
    void ResetGameSession()
    {
        FindObjectOfType<ScenePersist>().ResetScenePersist();
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }
}
