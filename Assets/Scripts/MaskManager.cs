using UnityEngine;

public class MaskManager : MonoBehaviour
{
    [Header("Test Setup")]
    public MaskData testMask; // Drag una MaskData qui per testare
    public Transform pedestal; // Un solo piedistallo per ora
    public GameObject defaultMaskPrefab; // Prefab di fallback se la MaskData non ha un modello
    
    private GameObject currentMaskInstance;
    
    void Start()
    {
        SpawnTestMask();
    }
    
    private void SpawnTestMask()
    {
        if (testMask == null || pedestal == null)
        {
            Debug.LogWarning("MaskManager: Missing references!");
            return;
        }
        
        // Scegli quale prefab usare
        GameObject prefabToUse = testMask.mask3DModel != null ? testMask.mask3DModel : defaultMaskPrefab;
        
        if (prefabToUse == null)
        {
            Debug.LogError("MaskManager: No mask prefab available!");
            return;
        }
        
        // Spawna la maschera sopra il piedistallo
        Vector3 spawnPos = pedestal.position + Vector3.up;
        currentMaskInstance = Instantiate(prefabToUse, spawnPos, Quaternion.identity);
        
        // Aggiungi l'interazione
        MaskInteractable interactable = currentMaskInstance.GetComponent<MaskInteractable>();
        if (interactable == null)
        {
            interactable = currentMaskInstance.AddComponent<MaskInteractable>();
        }
        interactable.Initialize(testMask);
        
        Debug.Log($"Spawned test mask: {testMask.maskName}");
    }

}