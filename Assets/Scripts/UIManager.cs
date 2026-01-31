using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Timer UI")]
    public Text uiTimer;
    public Text addTimer;

    [Header("Score UI")]
    public Text uiCounter;

    [Header("Touch Controls")]
    public GameObject touchControlsPanel;
    public Button jumpButton;
    public Button attackButton;
    public Button leftButton;
    public Button rightButton;

    private static UIManager _instance;
    public static UIManager Instance { get { return _instance; } }
    
    private void Awake()
    {
        if (_instance != null && _instance != this) Destroy(this.gameObject);
        else _instance = this;
    }

    // Timer UI Methods
    public void UpdateTimerDisplay(float timer)
    {
        if (timer < 0)
        {
            uiTimer.text = "GAME OVER";
        } 
        else 
        {
            uiTimer.text = timer.ToString("f1") + " SECONDS LEFT";
        }
    }

    public void ShowAddTimer(float addTime)
    {
        addTimer.gameObject.SetActive(true);
        addTimer.text = "+" + addTime.ToString();
        StartCoroutine(HideAddTimerAfterDelay());
    }

    private IEnumerator HideAddTimerAfterDelay()
    {
        yield return new WaitForSeconds(1);
        addTimer.gameObject.SetActive(false);
    }

    // Score UI Methods
    public void UpdateCounter(int counter)
    {
        uiCounter.text = counter + " COMPUTERS REPAIRED";
    }

    // Touch Controls Methods
    public void ShowTouchControls()
    {
        if (touchControlsPanel != null)
            touchControlsPanel.SetActive(true);
    }

    public void HideTouchControls()
    {
        if (touchControlsPanel != null)
            touchControlsPanel.SetActive(false);
    }

    public void ToggleTouchControls()
    {
        if (touchControlsPanel != null)
            touchControlsPanel.SetActive(!touchControlsPanel.activeSelf);
    }

    // Auto-detect if we're on mobile and show touch controls
    void Start()
    {
        #if UNITY_ANDROID || UNITY_IOS
            ShowTouchControls();
        #else
            // On desktop, hide touch controls by default
            HideTouchControls();
        #endif
    }

    // Handle input for toggling touch controls
    void Update()
    {
        // Toggle touch controls with E key (primarily for desktop testing)
        if (Input.GetKeyDown(KeyCode.E))
        {
            ToggleTouchControls();
        }
    }
}