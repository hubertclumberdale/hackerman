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
    [Header("Audio")]
    public AudioClip softSong;
    public AudioClip metalSong;
    public AudioClip gameOverSong;
    private AudioSource audioSource;

    [Header("Game State")]
    public GameState currentGameState = GameState.Playing;
    private int computersRepaired = 0;
    private bool gameStarted = false;

    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }
    
    private void Awake()
    {
        if (_instance != null && _instance != this) Destroy(this.gameObject);
        else _instance = this;
    }

    void Start() {
        audioSource = GetComponent<AudioSource>();
        SetGameState(GameState.Playing);
        PlayClip(softSong);
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
            computersRepaired++;
            TimeManager.Instance.UpdateCounter(computersRepaired);
        }
    }

    public void OnTimerFinished()
    {
        if (currentGameState == GameState.Playing)
        {
            SetGameState(GameState.GameOver);
            PlayGameOverSong();
        }
    }

    private void ShowGameOverScreen()
    {
        // Qui potrai aggiungere la UI della schermata di game over
        Debug.Log($"GAME OVER! Computer riparati: {computersRepaired}");
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public int GetComputersRepaired()
    {
        return computersRepaired;
    }

    public bool IsGameActive()
    {
        return currentGameState == GameState.Playing;
    }

    // Audio Methods
    public void PlaySoftSong()
    {
        PlayClip(softSong);
    }

    public void PlayMetalSong()
    {
        PlayClip(metalSong);
    }

    public void PlayGameOverSong(){
        PlayClip(gameOverSong);
    }

     void PlayClip(AudioClip clip){
        audioSource.clip = clip;
        audioSource.Play();
    }
}
