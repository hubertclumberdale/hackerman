using UnityEngine;

[CreateAssetMenu(fileName = "New Mask", menuName = "Masks/Mask Data")]
public class MaskData : ScriptableObject
{
    [Header("Basic Info")]
    public string maskName;
    public GameObject mask3DModel; // Il modello 3D della maschera
    
    [Header("Visual Settings")]
    public Color maskColor = Color.white; // Color to apply to the 3D model material
    
    [Header("Buff Effects")]
    public float speedMultiplier = 1.5f;
}