using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    public Button touchToggleButton;

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
            uiTimer.text = timer.ToString("f1") + " time left";
        }
    }

    public void ShowAddTimer(float addTime)
    {
        addTimer.gameObject.SetActive(true);
        addTimer.text = "+" + addTime.ToString() + "s";
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
        {
            touchControlsPanel.SetActive(true);
            UpdateToggleButtonState(true);
        }
    }

    public void HideTouchControls()
    {
        if (touchControlsPanel != null)
        {
            touchControlsPanel.SetActive(false);
            UpdateToggleButtonState(false);
        }
    }

    public void ToggleTouchControls()
    {
        if (touchControlsPanel != null)
        {
            bool newState = !touchControlsPanel.activeSelf;
            touchControlsPanel.SetActive(newState);
            UpdateToggleButtonState(newState);
        }
    }

    void Start()
    {
        UpdateToggleButtonState(false);
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
    
    // Helper method to update toggle button state
    void UpdateToggleButtonState(bool touchControlsActive)
    {
        if (touchToggleButton != null)
        {
            TextMeshProUGUI buttonText = touchToggleButton.GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null)
            {
                buttonText.text = touchControlsActive ? "[ON]" : "[OFF]";
            }
            
            Image buttonImage = touchToggleButton.GetComponent<Image>();
            if (buttonImage != null)
            {
                buttonImage.color = touchControlsActive ? new Color(0.8f, 1f, 0.8f, 0.8f) : new Color(1f, 0.8f, 0.8f, 0.8f);
            }
        }
    }
    
}