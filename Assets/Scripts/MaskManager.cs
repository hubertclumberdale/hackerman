using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class MaskManager : MonoBehaviour
{
    [Header("Setup")]
    public MaskData[] masks; // Array delle maschere disponibili
    public Transform[] pedestals; // I 3 piedistalli
    public GameObject defaultMaskPrefab; // Prefab di fallback se la MaskData non ha un modello
    
    [Header("UI")]
    public GameObject textPrefab; // Prefab for displaying mask names (should have TextMeshPro component)
    
    private GameObject[] currentMaskInstances = new GameObject[3];
    private GameObject[] currentTextInstances = new GameObject[3];
    
    void Start()
    {
        SpawnMasksOnPedestals();
    }
    
    private void SpawnMasksOnPedestals()
    {
        if (masks == null || masks.Length == 0)
        {
            Debug.LogWarning("MaskManager: No masks provided!");
            return;
        }
        
        if (pedestals == null || pedestals.Length != 3)
        {
            Debug.LogWarning("MaskManager: Pedestals array must contain exactly 3 pedestals!");
            return;
        }
        
        // Spawna maschere sui piedistalli
        for (int i = 0; i < pedestals.Length; i++)
        {
            SpawnMaskOnPedestal(i);
        }
    }
    
    private void SpawnMaskOnPedestal(int pedestalIndex)
    {
        if (pedestalIndex < 0 || pedestalIndex >= pedestals.Length)
        {
            Debug.LogError($"MaskManager: Invalid pedestal index {pedestalIndex}");
            return;
        }
        
        // Scegli una maschera casuale dall'array
        MaskData randomMask = masks[Random.Range(0, masks.Length)];
        Transform pedestal = pedestals[pedestalIndex];
        
        // Scegli quale prefab usare
        GameObject prefabToUse = randomMask.mask3DModel != null ? randomMask.mask3DModel : defaultMaskPrefab;
        
        if (prefabToUse == null)
        {
            Debug.LogError($"MaskManager: No mask prefab available for pedestal {pedestalIndex}!");
            return;
        }
        
        // Spawna la maschera sopra il piedistallo
        Vector3 spawnPos = pedestal.position + Vector3.up;
        // it needs to rotate 90 degrees on the Y axis to face forward
        currentMaskInstances[pedestalIndex] = Instantiate(prefabToUse, spawnPos, Quaternion.Euler(-90, -90, 270));
        
        // Apply color to the material
        ApplyColorToMask(currentMaskInstances[pedestalIndex], randomMask.maskColor);
        
        // Create text display for mask name
        CreateMaskNameText(pedestalIndex, randomMask.maskName, spawnPos);
        
        // Aggiungi l'interazione
        MaskInteractable interactable = currentMaskInstances[pedestalIndex].GetComponent<MaskInteractable>();
        if (interactable == null)
        {
            interactable = currentMaskInstances[pedestalIndex].AddComponent<MaskInteractable>();
        }
        interactable.Initialize(randomMask);
        
        Debug.Log($"Spawned mask '{randomMask.maskName}' on pedestal {pedestalIndex + 1} with color {randomMask.maskColor}");
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
    
    private void CreateMaskNameText(int pedestalIndex, string maskName, Vector3 maskPosition)
    {
        if (textPrefab == null)
        {
            // Create a simple text object if no prefab is provided
            CreateSimpleTextObject(pedestalIndex, maskName, maskPosition);
            return;
        }
        
        // Use the provided text prefab
        Vector3 textPosition = maskPosition + Vector3.back * 1.2f + Vector3.down * 0.3f; // Position in front and slightly below the mask
        currentTextInstances[pedestalIndex] = Instantiate(textPrefab, textPosition, Quaternion.identity);
        
        // Set the text content
        TextMeshPro textComponent = currentTextInstances[pedestalIndex].GetComponent<TextMeshPro>();
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
    
    private void CreateSimpleTextObject(int pedestalIndex, string maskName, Vector3 maskPosition)
    {
        // Create a new GameObject for the text
        GameObject textObject = new GameObject($"MaskText_{pedestalIndex}");
        Vector3 textPosition = maskPosition + Vector3.back * 1.2f + Vector3.down * 0.3f;
        textObject.transform.position = textPosition;
        
        // Add TextMeshPro component
        TextMeshPro textComponent = textObject.AddComponent<TextMeshPro>();
        textComponent.text = maskName;
        textComponent.fontSize = 2f;
        textComponent.color = Color.white;
        textComponent.alignment = TextAlignmentOptions.Center;
        textComponent.sortingOrder = 1; // Ensure text appears in front
        
        currentTextInstances[pedestalIndex] = textObject;
    }

}