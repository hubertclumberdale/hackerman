using UnityEngine;
using System.Collections.Generic;

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
    
    [Header("Visual Mask Management")]
    private GameObject currentMaskVisual; // Current mask GameObject on player's face
    private Material[] maskMaterials; // Materials of the current mask for color changes
    
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
        
        // Initialize mask (disabled by default)
        InitializeMask();
    }
    
    private void InitializeMask()
    {
        if (playerScript.maskOnFace != null)
        {
            playerScript.maskOnFace.SetActive(false);
            Debug.Log("Mask initialized and disabled on player");
        }
        else
        {
            Debug.LogWarning("No mask GameObject assigned to Player.maskOnFace! Please assign a mask prefab to the player.");
        }
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
        
        // Show mask visually on player's face
        ShowMaskOnPlayer(maskData);
        
        // Play player buff sound through AudioManager
        AudioManager.Instance.PlayPlayerBuffSound();
        
        Debug.Log($"Applied mask: {maskData.maskName} - Speed: x{maskData.speedMultiplier}");
    }
    
    public void RemoveCurrentMask()
    {
        if (!hasActiveMask) return;
        
        // Ripristina valori originali
        playerScript.speed = originalSpeed;
        
        // Hide mask visually
        HideMaskOnPlayer();
        
        Debug.Log($"Removed mask: {currentMask.maskName}");
        
        hasActiveMask = false;
        currentMask = null;
    }
    
    private void ShowMaskOnPlayer(MaskData maskData)
    {
        // Get the mask GameObject from the player
        currentMaskVisual = playerScript.maskOnFace;
        
        if (currentMaskVisual == null)
        {
            Debug.LogWarning("No mask GameObject found on player! Make sure to assign maskOnFace in the Player component.");
            return;
        }
        
        // Enable the mask GameObject
        currentMaskVisual.SetActive(true);
        
        // Apply the color from the mask data
        ApplyColorToMask(maskData.maskColor);
        
        Debug.Log($"Showing mask '{maskData.maskName}' on player with color {maskData.maskColor}");
    }
    
    private void HideMaskOnPlayer()
    {
        if (currentMaskVisual != null)
        {
            currentMaskVisual.SetActive(false);
            currentMaskVisual = null;
            maskMaterials = null;
            Debug.Log("Hidden mask from player");
        }
    }
    
    private void ApplyColorToMask(Color color)
    {
        if (currentMaskVisual == null) return;
        
        // Get all renderers in the mask object (including children)
        Renderer[] renderers = currentMaskVisual.GetComponentsInChildren<Renderer>();
        
        // Store materials for potential future changes
        if (maskMaterials == null)
        {
            List<Material> materialsList = new List<Material>();
            foreach (Renderer renderer in renderers)
            {
                materialsList.AddRange(renderer.materials);
            }
            maskMaterials = materialsList.ToArray();
        }
        
        foreach (Renderer renderer in renderers)
        {
            // Apply color to all materials
            foreach (Material material in renderer.materials)
            {
                // Try to set the main color property (most common properties)
                if (material.HasProperty("_Color"))
                {
                    material.color = color;
                }
                else if (material.HasProperty("_BaseColor"))
                {
                    material.SetColor("_BaseColor", color);
                }
                else if (material.HasProperty("_MainColor"))
                {
                    material.SetColor("_MainColor", color);
                }
            }
        }
    }
}