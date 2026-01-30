using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    public float timer = 20;

    private static TimeManager _instance;
    public static TimeManager Instance { get { return _instance; } }
    private void Awake()
    {
        if (_instance != null && _instance != this) Destroy(this.gameObject);
        else _instance = this;
    }


    private void Start()
    {
        Time.timeScale = 1;
    }

    private void Update()
    {
        if (TileManager.Instance.rooms.Count > 1 && GameManager.Instance.IsGameActive())
        { 
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                TimerFinished();
                GameManager.Instance.OnTimerFinished();
            }
            UIManager.Instance.UpdateTimerDisplay(timer);
        }
    }

    public void AddTimer(float addTime)
    {
        if (GameManager.Instance.IsGameActive())
        {
            timer += addTime;
            UIManager.Instance.ShowAddTimer(addTime);
        }
    }

    public void TimerFinished()
    {
        // Il controllo del game over è ora gestito dal GameManager
    }
}
