using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TouchControlManager : MonoBehaviour
{
    [Header("Touch Control References")]
    public Button jumpButton;
    public Button attackButton;
    public Button leftButton;
    public Button rightButton;
    
    [Header("Player Reference")]
    public Player playerController;
    
    // Input states
    private bool isMovingLeft = false;
    private bool isMovingRight = false;
    private bool jumpPressed = false;
    private bool attackPressed = false;
    
    private static TouchControlManager _instance;
    public static TouchControlManager Instance { get { return _instance; } }
    
    private void Awake()
    {
        if (_instance != null && _instance != this) Destroy(this.gameObject);
        else _instance = this;
    }

    void Start()
    {
        SetupTouchControls();
        
        // Find player if not assigned
        if (playerController == null)
        {
            playerController = FindObjectOfType<Player>();
        }
    }
    
    void SetupTouchControls()
    {
        // Setup Jump Button
        if (jumpButton != null)
        {
            SetupButton(jumpButton, OnJumpPress, OnJumpRelease);
        }
        
        // Setup Attack Button
        if (attackButton != null)
        {
            SetupButton(attackButton, OnAttackPress, OnAttackRelease);
        }
        
        // Setup Left Button
        if (leftButton != null)
        {
            SetupButton(leftButton, OnLeftPress, OnLeftRelease);
        }
        
        // Setup Right Button
        if (rightButton != null)
        {
            SetupButton(rightButton, OnRightPress, OnRightRelease);
        }
    }
    
    void SetupButton(Button button, UnityEngine.Events.UnityAction onPress, UnityEngine.Events.UnityAction onRelease)
    {
        EventTrigger trigger = button.gameObject.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = button.gameObject.AddComponent<EventTrigger>();
        }
        
        // Pointer Down Event
        EventTrigger.Entry pointerDown = new EventTrigger.Entry();
        pointerDown.eventID = EventTriggerType.PointerDown;
        pointerDown.callback.AddListener((data) => { onPress(); });
        trigger.triggers.Add(pointerDown);
        
        // Pointer Up Event
        EventTrigger.Entry pointerUp = new EventTrigger.Entry();
        pointerUp.eventID = EventTriggerType.PointerUp;
        pointerUp.callback.AddListener((data) => { onRelease(); });
        trigger.triggers.Add(pointerUp);
        
        // Pointer Exit Event (per quando il dito esce dal bottone)
        EventTrigger.Entry pointerExit = new EventTrigger.Entry();
        pointerExit.eventID = EventTriggerType.PointerExit;
        pointerExit.callback.AddListener((data) => { onRelease(); });
        trigger.triggers.Add(pointerExit);
    }
    
    // Input state methods for Player script to access
    public bool GetJumpInput()
    {
        bool result = jumpPressed;
        jumpPressed = false; // Reset after reading
        return result;
    }
    
    public bool GetAttackInput()
    {
        bool result = attackPressed;
        attackPressed = false; // Reset after reading
        return result;
    }
    
    public float GetHorizontalInput()
    {
        if (isMovingLeft && !isMovingRight)
            return -1f;
        else if (isMovingRight && !isMovingLeft)
            return 1f;
        else
            return 0f;
    }
    
    // Button event handlers
    void OnJumpPress()
    {
        jumpPressed = true;
    }
    
    void OnJumpRelease()
    {
        // Jump doesn't need hold functionality
    }
    
    void OnAttackPress()
    {
        attackPressed = true;
    }
    
    void OnAttackRelease()
    {
        // Attack doesn't need hold functionality
    }
    
    void OnLeftPress()
    {
        isMovingLeft = true;
    }
    
    void OnLeftRelease()
    {
        isMovingLeft = false;
    }
    
    void OnRightPress()
    {
        isMovingRight = true;
    }
    
    void OnRightRelease()
    {
        isMovingRight = false;
    }
    
    // Method to check if touch controls are being used
    public bool IsTouchControlActive()
    {
        return isMovingLeft || isMovingRight;
    }
}