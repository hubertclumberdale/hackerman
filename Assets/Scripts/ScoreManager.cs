using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private int computersRepaired = 0;

    private static ScoreManager _instance;
    public static ScoreManager Instance { get { return _instance; } }
    
    private void Awake()
    {
        if (_instance != null && _instance != this) Destroy(this.gameObject);
        else _instance = this;
    }

    public void OnComputerRepaired()
    {
        if (GameManager.Instance.IsGameActive())
        {
            computersRepaired++;
            UIManager.Instance.UpdateCounter(computersRepaired);
        }
    }

    public int GetComputersRepaired()
    {
        return computersRepaired;
    }

    public void ResetScore()
    {
        computersRepaired = 0;
    }
}