using UnityEngine;

public class PlayerBuffs : MonoBehaviour
{
    public bool hasActiveMask = false;
    public MaskData currentMask; // Traccia la maschera attuale
    
    // Singleton
    private static PlayerBuffs _instance;
    public static PlayerBuffs Instance { get { return _instance; } }
    
    // References
    private Player playerScript;
    private float originalSpeed;
    
    private void Awake()
    {
        if (_instance != null && _instance != this) 
        {
            Destroy(this.gameObject);
            return;
        }
        _instance = this;
    }
    
    void Start()
    {
        playerScript = GetComponent<Player>();
        originalSpeed = playerScript.speed;
    }
    
    public void ApplyMaskBuff(MaskData maskData)
    {
        // Se c'è già una maschera, rimuovila prima
        if (hasActiveMask)
        {
            RemoveCurrentMask();
        }
        
        // Applica la nuova maschera
        currentMask = maskData;
        hasActiveMask = true;
        playerScript.speed = originalSpeed * maskData.speedMultiplier;
        
        // Play mask pickup sound through AudioManager
        AudioManager.Instance.PlayMaskPickupSound();
        
        Debug.Log($"Applied mask: {maskData.maskName} - Speed: x{maskData.speedMultiplier}");
    }
    
    public void RemoveCurrentMask()
    {
        if (!hasActiveMask) return;
        
        // Ripristina valori originali
        playerScript.speed = originalSpeed;
        
        Debug.Log($"Removed mask: {currentMask.maskName}");
        
        hasActiveMask = false;
        currentMask = null;
    }
}