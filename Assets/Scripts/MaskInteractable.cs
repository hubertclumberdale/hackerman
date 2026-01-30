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
        
        SelectMask();
    }
    
    private void SelectMask()
    {
        canInteract = false; // Previeni multiple selezioni
        
        Debug.Log($"Mask hit by mazza: {maskData.maskName}");
        
        PlayerBuffs.Instance.ApplyMaskBuff(maskData);
    }
}