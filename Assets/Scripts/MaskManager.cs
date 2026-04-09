using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class MaskManager : MonoBehaviour
{
    [Header("Setup")]
    public MaskData[] masks; // Array delle maschere disponibili
    public Transform pedestal; // Il piedistallo
    public GameObject defaultMaskPrefab; // Prefab di fallback se la MaskData non ha un modello
    
    [Header("UI")]
    public GameObject textPrefab; // Prefab for displaying mask names (should have TextMeshPro component)
    
    private GameObject currentMaskInstance;
    private GameObject currentTextInstance;
    
    void Start()
    {
        SpawnRandomMask();
    }
    
    public void SpawnRandomMask()
    {
        if (masks == null || masks.Length == 0)
        {
            Debug.LogWarning("MaskManager: No masks provided!");
            return;
        }
        
        if (pedestal == null)
        {
            Debug.LogWarning("MaskManager: No pedestal assigned!");
            return;
        }
        
        // Rimuovi la maschera precedente se esiste
        ClearCurrentMask();
        
        // Spawna una nuova maschera
        SpawnMaskOnPedestal();
    }
    
    private void SpawnMaskOnPedestal()
    {
        // Scegli una maschera casuale dall'array
        MaskData randomMask = masks[Random.Range(0, masks.Length)];
        
        // Scegli quale prefab usare
        GameObject prefabToUse = randomMask.mask3DModel != null ? randomMask.mask3DModel : defaultMaskPrefab;
        
        if (prefabToUse == null)
        {
            Debug.LogError("MaskManager: No mask prefab available!");
            return;
        }
        
        // Spawna la maschera sopra il piedistallo
        Vector3 spawnPos = pedestal.position + Vector3.up;
        // it needs to rotate 90 degrees on the Y axis to face forward
        currentMaskInstance = Instantiate(prefabToUse, spawnPos, Quaternion.Euler(-90, -90, 270));
        
        // Apply color to the material
        ApplyColorToMask(currentMaskInstance, randomMask.maskColor);
        
        // Create text display for mask description
        //either use description or use mask name
        string maskDescription = !string.IsNullOrEmpty(randomMask.description) ? randomMask.description : randomMask.maskName;  

        CreateMaskDescriptionText(maskDescription, spawnPos);
        
        // Aggiungi l'interazione
        MaskInteractable interactable = currentMaskInstance.GetComponent<MaskInteractable>();
        if (interactable == null)
        {
            interactable = currentMaskInstance.AddComponent<MaskInteractable>();
        }
        interactable.Initialize(randomMask);
        
        Debug.Log($"Spawned mask '{randomMask.description}' with color {randomMask.maskColor}");
    }

    private void ApplyColorToMask(GameObject maskObject, Color color)
    {
        // Get all renderers in the mask object (including children)
        Renderer[] renderers = maskObject.GetComponentsInChildren<Renderer>();
        
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
    
    private void CreateMaskDescriptionText(string maskName, Vector3 maskPosition)
    {
        if (textPrefab == null)
        {
            // Create a simple text object if no prefab is provided
            CreateSimpleTextObject(maskName, maskPosition);
            return;
        }
        
        // Use the provided text prefab
        Vector3 textPosition = maskPosition + Vector3.back * 1.2f + Vector3.down * 0.3f; // Position in front and slightly below the mask
        currentTextInstance = Instantiate(textPrefab, textPosition, Quaternion.identity);
        
        // Set text as child of the mask
        currentTextInstance.transform.SetParent(currentMaskInstance.transform);
        
        // Set the text content
        TextMeshPro textComponent = currentTextInstance.GetComponent<TextMeshPro>();
        if (textComponent != null)
        {
            textComponent.text = maskName;
            textComponent.fontSize = 2f;
            textComponent.color = Color.white;
            textComponent.alignment = TextAlignmentOptions.Center;
        }
        else
        {
            Debug.LogWarning("Text prefab doesn't have a TextMeshPro component!");
        }
    }
    
    private void CreateSimpleTextObject(string maskName, Vector3 maskPosition)
    {
        // Create a new GameObject for the text
        GameObject textObject = new GameObject("MaskText");
        Vector3 textPosition = maskPosition + Vector3.back * 1.2f + Vector3.down * 0.3f;
        textObject.transform.position = textPosition;
        
        // Set text as child of the mask
        textObject.transform.SetParent(currentMaskInstance.transform);
        
        // Add TextMeshPro component
        TextMeshPro textComponent = textObject.AddComponent<TextMeshPro>();
        textComponent.text = maskName;
        textComponent.fontSize = 2f;
        textComponent.color = Color.white;
        textComponent.alignment = TextAlignmentOptions.Center;
        textComponent.sortingOrder = 1; // Ensure text appears in front
        
        currentTextInstance = textObject;
    }
    
    private void ClearCurrentMask()
    {
        if (currentMaskInstance != null)
        {
            DestroyImmediate(currentMaskInstance);
            currentMaskInstance = null;
        }
        
        if (currentTextInstance != null)
        {
            DestroyImmediate(currentTextInstance);
            currentTextInstance = null;
        }
    }

}