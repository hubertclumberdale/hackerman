using UnityEngine;
using TMPro;
using System.Collections;

public class TextPopup : MonoBehaviour
{
    [Header("Popup Settings")]
    public float animationDuration = 1.5f;
    public float moveUpDistance = 2f;
    public AnimationCurve fadeCurve = AnimationCurve.EaseInOut(0, 1, 1, 0);
    public AnimationCurve scaleCurve = AnimationCurve.EaseInOut(0, 0.5f, 0.3f, 1.2f);
    
    private TextMeshPro textMeshPro;
    private Vector3 startPosition;
    private Color startColor;
    
    private void Awake()
    {
        textMeshPro = GetComponent<TextMeshPro>();
        if (textMeshPro == null)
        {
            textMeshPro = gameObject.AddComponent<TextMeshPro>();
        }
        
        startPosition = transform.position;
        startColor = textMeshPro.color;
    }
    
    public void Initialize(string text, Vector3 position, Color color = default)
    {
        if (color == default)
            color = Color.white;
            
        textMeshPro.text = text;
        textMeshPro.fontSize = 3f;
        textMeshPro.color = color;
        textMeshPro.alignment = TextAlignmentOptions.Center;
        textMeshPro.sortingOrder = 10; // Ensure text appears in front
        
        transform.position = position;
        startPosition = position;
        startColor = color;
        
        StartCoroutine(PlayPopupAnimation());
    }
    
    private IEnumerator PlayPopupAnimation()
    {
        float elapsedTime = 0f;
        Vector3 endPosition = startPosition + Vector3.up * moveUpDistance;
        
        while (elapsedTime < animationDuration)
        {
            float progress = elapsedTime / animationDuration;
            
            // Animate position
            transform.position = Vector3.Lerp(startPosition, endPosition, progress);
            
            // Animate scale
            float scaleValue = scaleCurve.Evaluate(progress);
            transform.localScale = Vector3.one * scaleValue;
            
            // Animate alpha
            float alpha = fadeCurve.Evaluate(progress);
            Color currentColor = startColor;
            currentColor.a = alpha;
            textMeshPro.color = currentColor;
            
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        // Cleanup - destroy the popup
        Destroy(gameObject);
    }
    
    // Static method to create a popup easily
    public static TextPopup Create(string text, Vector3 position, Color color = default)
    {
        GameObject popupObj = new GameObject("TextPopup");
        TextPopup popup = popupObj.AddComponent<TextPopup>();
        popup.Initialize(text, position, color);
        return popup;
    }
}