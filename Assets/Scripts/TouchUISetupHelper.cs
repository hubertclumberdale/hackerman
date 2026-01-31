using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class TouchButtonStyle
{
    public Color normalColor = Color.white;
    public Color pressedColor = Color.gray;
    public Color disabledColor = Color.gray;
    public float fadeMultiplier = 1.2f;
}

public class TouchUISetupHelper : MonoBehaviour
{
    [Header("Button Styling")]
    public TouchButtonStyle buttonStyle;
    
    [Header("Button Sizes")]
    public Vector2 movementButtonSize = new Vector2(80, 80);
    public Vector2 actionButtonSize = new Vector2(70, 70);
    
    [Header("Button Positions (Anchor Percentages 0-1)")]
    [Range(0f, 1f)] public float leftButtonX = 0.1f;
    [Range(0f, 1f)] public float leftButtonY = 0.15f;
    [Range(0f, 1f)] public float rightButtonX = 0.25f;
    [Range(0f, 1f)] public float rightButtonY = 0.15f;
    [Range(0f, 1f)] public float jumpButtonX = 0.85f;
    [Range(0f, 1f)] public float jumpButtonY = 0.15f;
    [Range(0f, 1f)] public float attackButtonX = 0.9f;
    [Range(0f, 1f)] public float attackButtonY = 0.3f;
    
    [Header("Auto Setup")]
    public bool autoSetupOnStart = true;
    public Canvas targetCanvas;
    
    void Start()
    {
        if (autoSetupOnStart)
        {
            SetupTouchUI();
        }
    }
    
    [ContextMenu("Setup Touch UI")]
    public void SetupTouchUI()
    {
        if (targetCanvas == null)
        {
            targetCanvas = FindObjectOfType<Canvas>();
            if (targetCanvas == null)
            {
                Debug.LogError("No Canvas found in scene!");
                return;
            }
        }
        
        // Create main touch controls panel
        GameObject touchPanel = CreateTouchControlsPanel();
        
        // Create buttons
        Button leftBtn = CreateTouchButton("LeftButton", "←", movementButtonSize, leftButtonX, leftButtonY, touchPanel.transform);
        Button rightBtn = CreateTouchButton("RightButton", "→", movementButtonSize, rightButtonX, rightButtonY, touchPanel.transform);
        Button jumpBtn = CreateTouchButton("JumpButton", "↑", actionButtonSize, jumpButtonX, jumpButtonY, touchPanel.transform);
        Button attackBtn = CreateTouchButton("AttackButton", "⚔", actionButtonSize, attackButtonX, attackButtonY, touchPanel.transform);
        
        // Setup UIManager references
        UIManager uiManager = FindObjectOfType<UIManager>();
        if (uiManager != null)
        {
            uiManager.touchControlsPanel = touchPanel;
            uiManager.leftButton = leftBtn;
            uiManager.rightButton = rightBtn;
            uiManager.jumpButton = jumpBtn;
            uiManager.attackButton = attackBtn;
        }
        
        // Setup TouchControlManager references
        TouchControlManager touchManager = FindObjectOfType<TouchControlManager>();
        if (touchManager != null)
        {
            touchManager.leftButton = leftBtn;
            touchManager.rightButton = rightBtn;
            touchManager.jumpButton = jumpBtn;
            touchManager.attackButton = attackBtn;
        }
        
        Debug.Log("Touch UI setup completed!");
    }
    
    GameObject CreateTouchControlsPanel()
    {
        GameObject panel = new GameObject("TouchControlsPanel");
        panel.transform.SetParent(targetCanvas.transform, false);
        
        RectTransform rectTransform = panel.AddComponent<RectTransform>();
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;
        
        return panel;
    }
    
    Button CreateTouchButton(string name, string text, Vector2 size, float posX, float posY, Transform parent)
    {
        // Create button GameObject
        GameObject buttonObj = new GameObject(name);
        buttonObj.transform.SetParent(parent, false);
        
        // Add RectTransform
        RectTransform rectTransform = buttonObj.AddComponent<RectTransform>();
        rectTransform.sizeDelta = size;
        rectTransform.anchorMin = new Vector2(posX, posY);
        rectTransform.anchorMax = new Vector2(posX, posY);
        rectTransform.anchoredPosition = Vector2.zero;
        
        // Add Image component
        Image image = buttonObj.AddComponent<Image>();
        image.color = buttonStyle.normalColor;
        
        // Add Button component
        Button button = buttonObj.AddComponent<Button>();
        
        // Setup button colors
        ColorBlock colorBlock = button.colors;
        colorBlock.normalColor = buttonStyle.normalColor;
        colorBlock.pressedColor = buttonStyle.pressedColor;
        colorBlock.disabledColor = buttonStyle.disabledColor;
        colorBlock.colorMultiplier = buttonStyle.fadeMultiplier;
        button.colors = colorBlock;
        
        // Create text child
        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(buttonObj.transform, false);
        
        Text textComponent = textObj.AddComponent<Text>();
        textComponent.text = text;
        textComponent.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        textComponent.fontSize = Mathf.RoundToInt(size.x * 0.4f);
        textComponent.alignment = TextAnchor.MiddleCenter;
        textComponent.color = Color.black;
        
        RectTransform textRect = textComponent.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;
        
        return button;
    }
    
    [ContextMenu("Remove Touch UI")]
    public void RemoveTouchUI()
    {
        GameObject existingPanel = GameObject.Find("TouchControlsPanel");
        if (existingPanel != null)
        {
            DestroyImmediate(existingPanel);
            Debug.Log("Touch UI removed!");
        }
    }
}