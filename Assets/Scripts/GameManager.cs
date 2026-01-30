using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    Playing,
    GameOver,
    Paused
}

public class GameManager : MonoBehaviour
{
    [Header("Game State")]
    public GameState currentGameState = GameState.Playing;
    private bool gameStarted = false;

    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }
    
    private void Awake()
    {
        if (_instance != null && _instance != this) Destroy(this.gameObject);
        else _instance = this;
    }

    void Start() {
        SetGameState(GameState.Playing);
    }

    public void SetGameState(GameState newState)
    {
        currentGameState = newState;
        
        switch (newState)
        {
            case GameState.Playing:
                Time.timeScale = 1f;
                break;
            case GameState.GameOver:
                Time.timeScale = 0f;
                ShowGameOverScreen();
                break;
            case GameState.Paused:
                Time.timeScale = 0f;
                break;
        }
    }

    public void OnComputerRepaired()
    {
        if (currentGameState == GameState.Playing)
        {
            ScoreManager.Instance.OnComputerRepaired();
        }
    }

    public void OnTimerFinished()
    {
        if (currentGameState == GameState.Playing)
        {
            SetGameState(GameState.GameOver);
            AudioManager.Instance.PlayGameOverSong();
        }
    }

    private void ShowGameOverScreen()
    {
        // Qui potrai aggiungere la UI della schermata di game over
        Debug.Log($"GAME OVER! Computer riparati: {ScoreManager.Instance.GetComputersRepaired()}");
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public int GetComputersRepaired()
    {
        return ScoreManager.Instance.GetComputersRepaired();
    }

    public bool IsGameActive()
    {
        return currentGameState == GameState.Playing;
    }

    // Audio methods moved to AudioManager
}
