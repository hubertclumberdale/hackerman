using UnityEngine;

[CreateAssetMenu(fileName = "New Mask", menuName = "Masks/Mask Data")]
public class MaskData : ScriptableObject
{
    [Header("Basic Info")]
    public string maskName;
    public string description;

    public GameObject mask3DModel;
    
    [Header("Visual Settings")]
    public Color maskColor = Color.white;
    
    [Header("Buff Effects")]
    [Tooltip("Multiplies player movement speed")]
    public float speedMultiplier = 1.5f;
    
    [Tooltip("Seconds added to the game timer when mask is collected")]
    public float countdownBonus = 5f;

    [Tooltip("Multiplies the size of the player's weapon (mazza)")]
    public float weaponSizeMultiplier = 1f;

}