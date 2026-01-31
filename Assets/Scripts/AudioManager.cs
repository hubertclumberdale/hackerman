using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Background Music")]
    public AudioClip softSong;
    public AudioClip metalSong;
    public AudioClip gameOverSong;
    public AudioClip shopSong;
    private AudioSource musicSource;

    [Header("Player Audio")]
    public bool stepsEnabled;
    public Object[] steps;
    public Object[] swearings;
    public float playerVolume = 1;
    private AudioSource playerAudioSource;

    [Header("Mazza Audio")]
    public Object[] hits;
    public float mazzaVolume = 1;
    private AudioSource mazzaAudioSource;

    [Header("Mask Audio")]
    public Object[] maskPickupSounds;
    public float maskVolume = 1;
    private AudioSource maskAudioSource;

    [Header("Computer Audio")]
    public Object[] computerRepairSounds;
    public float computerVolume = 1;
    private AudioSource computerAudioSource;

    private static AudioManager _instance;
    public static AudioManager Instance { get { return _instance; } }
    
    private void Awake()
    {
        if (_instance != null && _instance != this) Destroy(this.gameObject);
        else _instance = this;
    }

    void Start() 
    {
        // Create AudioSources automatically
        musicSource = gameObject.AddComponent<AudioSource>();
        playerAudioSource = gameObject.AddComponent<AudioSource>();
        mazzaAudioSource = gameObject.AddComponent<AudioSource>();
        maskAudioSource = gameObject.AddComponent<AudioSource>();
        computerAudioSource = gameObject.AddComponent<AudioSource>();
        
        // Configure music source
        musicSource.loop = true;
        musicSource.volume = 0.7f;
        
        // Configure player and mazza sources
        playerAudioSource.volume = playerVolume;
        mazzaAudioSource.volume = mazzaVolume;
        maskAudioSource.volume = maskVolume;
        computerAudioSource.volume = computerVolume;
        
        // Load audio resources
        steps = Resources.LoadAll("SFX/Steps", typeof(AudioClip));
        swearings = Resources.LoadAll("SFX/Imprecazioni", typeof(AudioClip));
        hits = Resources.LoadAll("SFX/Mazza", typeof(AudioClip));
        maskPickupSounds = Resources.LoadAll("SFX/Masks", typeof(AudioClip));
        computerRepairSounds = Resources.LoadAll("SFX/Malfunction", typeof(AudioClip));
        
        PlaySoftSong();
    }

    // Background Music Methods
    public void PlaySoftSong()
    {
        PlayMusicClip(softSong);
    }

    public void PlayMetalSong()
    {
        PlayMusicClip(metalSong);
    }

    public void PlayShopSong()
    {
        PlayMusicClip(shopSong);
    }

    public void PlayGameOverSong()
    {
        PlayMusicClip(gameOverSong);
    }

    void PlayMusicClip(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.Play();
    }

    // Player Audio Methods
    public void PlayMovementSound() 
    {
        if(stepsEnabled)
        {
            PlaySound(steps, playerAudioSource, playerVolume);
        }
    }

    public void PlaySwearingsSound() 
    {   
        PlaySound(swearings, playerAudioSource, playerVolume);
    }

    // Mazza Audio Methods
    public void PlayHit() 
    {
        PlaySound(hits, mazzaAudioSource, mazzaVolume);
    }

    // Mask Audio Methods
    public void PlayMaskPickupSound()
    {
        PlaySound(maskPickupSounds, maskAudioSource, maskVolume);
    }

    // Computer Audio Methods
    public void PlayComputerRepairSound()
    {
        PlaySound(computerRepairSounds, computerAudioSource, computerVolume);
    }

    private void PlaySound(Object[] array, AudioSource audioSource, float volume)
    {
        int randomRange = Random.Range(0, (array.Length-1));
        AudioClip arrayClip = array[randomRange] as AudioClip;
        audioSource.PlayOneShot(arrayClip, volume);
    }
}