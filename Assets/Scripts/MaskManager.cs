using UnityEngine;
using System.Collections.Generic;

public class MaskManager : MonoBehaviour
{
    [Header("Setup")]
    public MaskData[] masks; // Array delle maschere disponibili
    public Transform[] pedestals; // I 3 piedistalli
    public GameObject defaultMaskPrefab; // Prefab di fallback se la MaskData non ha un modello
    
    private GameObject[] currentMaskInstances = new GameObject[3];
    
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
        
        // Aggiungi l'interazione
        MaskInteractable interactable = currentMaskInstances[pedestalIndex].GetComponent<MaskInteractable>();
        if (interactable == null)
        {
            interactable = currentMaskInstances[pedestalIndex].AddComponent<MaskInteractable>();
        }
        interactable.Initialize(randomMask);
        
        Debug.Log($"Spawned mask '{randomMask.maskName}' on pedestal {pedestalIndex + 1}");
    }

}