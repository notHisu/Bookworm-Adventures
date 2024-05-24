using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

// Class to manage the game's audio
public class SoundManager : MonoBehaviour
{
    // AudioSource for the music
    public AudioSource musicSource;

    // List of AudioSources for the sound effects
    public List<AudioSource> sfxSources;

    // Booleans to track if the music and sound effects are on
    private bool musicOn = true;
    private bool sfxOn = true;

    // AudioMixer for controlling the volume
    [SerializeField]
    AudioMixer audioMixer;

    // Singleton instance of the SoundManager
    private static SoundManager instance;
    public static SoundManager Instance
    {
        get
        {
            // If the instance is null, find the SoundManager in the scene or create a new one
            if (instance == null)
            {
                instance = FindObjectOfType<SoundManager>();
                if (instance == null)
                {
                    Debug.LogError("No SoundManager found in scene. Creating instance.");
                    instance = new SoundManager();
                }
            }
            return instance;
        }
    }

    // Method called when the script instance is being loaded
    void Awake()
    {
        // If the instance is not null and not this, destroy this duplicate
        if (instance != null && instance != this)
        {
            DestroyImmediate(gameObject);
            return;
        }

        // Set the instance to this
        instance = this;

        // Find all the AudioSources in the scene and add them to the sfxSources list
        sfxSources = new List<AudioSource>(FindObjectsOfType<AudioSource>());

        // Remove the musicSource from the sfxSources list if it's there
        if (sfxSources.Contains(musicSource))
        {
            sfxSources.Remove(musicSource);
        }
    }

    // Method to play a sound
    public void PlaySound(AudioSource source, AudioClip clip)
    {
        // Set the clip of the source and play it
        source.clip = clip;
        source.Play();
    }

    // Method to toggle the music
    public void ToggleMusic(bool isOn)
    {
        // Set the musicOn boolean and mute the musicSource based on the isOn parameter
        musicOn = isOn;
        musicSource.mute = !isOn;
        // Save the state of the music in PlayerPrefs
        PlayerPrefs.SetInt("MusicOn", isOn ? 1 : 0);
    }

    // Method to toggle the sound effects
    public void ToggleSFX(bool isOn)
    {
        // Set the sfxOn boolean and mute all the sfxSources based on the isOn parameter
        sfxOn = isOn;
        foreach (var sfxSource in sfxSources)
        {
            sfxSource.mute = !isOn;
        }
        // Save the state of the sound effects in PlayerPrefs
        PlayerPrefs.SetInt("SFXOn", isOn ? 1 : 0);
    }

    // Method called before the first frame update
    void Start()
    {
        // Load the audio settings
        LoadAudioSettings();
    }

    // Method to load the audio settings
    private void LoadAudioSettings()
    {
        // Load the state of the music and sound effects from PlayerPrefs
        musicOn = PlayerPrefs.GetInt("MusicOn", 1) == 1;
        sfxOn = PlayerPrefs.GetInt("SFXOn", 1) == 1;

        // Mute the musicSource and all the sfxSources based on the loaded settings
        musicSource.mute = !musicOn;
        foreach (var sfxSource in sfxSources)
        {
            sfxSource.mute = !sfxOn;
        }
    }

    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("MasterVolume", volume);
        Debug.Log("Master: " + volume);
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", volume);
        Debug.Log("Music: " + volume);
    }
    public void SetEffectVolume(float volume)
    {
        audioMixer.SetFloat("EffectVolume", volume);
        Debug.Log("Effect: " + volume);
    }

}
