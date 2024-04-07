using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource musicSource;
    public List<AudioSource> sfxSources;

    private bool musicOn = true;
    private bool sfxOn = true;

    private static SoundManager instance;
    public static SoundManager Instance
    {
        get
        {
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

    void Awake()
    {
        if (instance != null && instance != this)
        {
            DestroyImmediate(gameObject); // Destroy duplicate if it exists
            return;
        }

        instance = this;

        sfxSources = new List<AudioSource>(FindObjectsOfType<AudioSource>());

        // Remove the musicSource from the sfxSources list if it's there
        if (sfxSources.Contains(musicSource))
        {
            sfxSources.Remove(musicSource);
        }
    }

    public void PlaySound(AudioSource source, AudioClip clip)
    {
        source.clip = clip;
        source.Play();
    }

    public void ToggleMusic(bool isOn)
    {
        musicOn = isOn;
        musicSource.mute = !isOn;
        PlayerPrefs.SetInt("MusicOn", isOn ? 1 : 0);
    }

    public void ToggleSFX(bool isOn)
    {
        sfxOn = isOn;
        foreach (var sfxSource in sfxSources)
        {
            sfxSource.mute = !isOn;
        }
        PlayerPrefs.SetInt("SFXOn", isOn ? 1 : 0);
    }

    void Start()
    {
        LoadAudioSettings();
    }

    private void LoadAudioSettings()
    {
        musicOn = PlayerPrefs.GetInt("MusicOn", 1) == 1;
        sfxOn = PlayerPrefs.GetInt("SFXOn", 1) == 1;

        musicSource.mute = !musicOn;
        foreach (var sfxSource in sfxSources)
        {
            sfxSource.mute = !sfxOn;
        }
    }
}
