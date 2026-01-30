using UnityEngine;

[CreateAssetMenu(fileName = "New Mask", menuName = "Masks/Mask Data")]
public class MaskData : ScriptableObject
{
    [Header("Basic Info")]
    public string maskName;
    public Sprite maskIcon;
    public GameObject mask3DModel; // Il modello 3D della maschera
    
    [Header("Buff Effects")]
    public float speedMultiplier = 1.5f;
}