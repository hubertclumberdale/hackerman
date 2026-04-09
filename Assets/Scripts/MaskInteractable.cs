using UnityEngine;

public class MaskInteractable : MonoBehaviour
{
    public MaskData maskData;
    public bool canInteract = true;
    
    public void Initialize(MaskData data)
    {
        maskData = data;
        
        // Assicurati che ci sia un collider per la collisione fisica
        Collider col = GetComponent<Collider>();
        if (col == null)
        {
            col = gameObject.AddComponent<BoxCollider>();
        }
        // NON è trigger perché deve collide fisicamente con la mazza
        col.isTrigger = false;
    }
    
    public void OnMazzaHit()
    {
        if (!canInteract) return;
        
        // Show popup with mask description before selecting
        ShowDescriptionPopup();
        
        SelectMask();
    }
    
    private void SelectMask()
    {
        canInteract = false; // Previeni multiple selezioni
        
        Debug.Log($"Mask hit by mazza: {maskData.maskName}");
        
        // Play mask pickup sound when mask is hit/collected
        AudioManager.Instance.PlayMaskSound();
        
        PlayerBuffs.Instance.ApplyMaskBuff(maskData);
        
        // Make the mask disappear
        Destroy(gameObject);
    }
    
    private void ShowDescriptionPopup()
    {
        Vector3 popupPosition = transform.position + Vector3.up;
        
        if (maskData == null || string.IsNullOrEmpty(maskData.description))
        {
            // Se non c'è descrizione, mostra il nome della maschera
            string displayText = maskData?.maskName ?? "Unknown Mask";
            TextPopup.Create(displayText, popupPosition, Color.yellow);
            return;
        }
        
        // Mostra la descrizione della maschera
        Color popupColor = maskData.maskColor != Color.white ? maskData.maskColor : Color.cyan;
        TextPopup.Create(maskData.description, popupPosition, popupColor);
    }
}