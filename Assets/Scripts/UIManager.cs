using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;

    [SerializeField]
    private Button startButton,
        menuButton,
        aboutButton,
        settingsMenuButton,
        backButton,
        scrambleButton,
        attackButton;

    [SerializeField]
    private AudioClip clickSound;

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private AudioClip scrambleSound;

    [SerializeField]
    private AudioClip attackSound;

    [SerializeField]
    private TMP_Text currentScoreText,
        bestScoreText;

    [SerializeField]
    private GameObject settingsPanel;

    [SerializeField]
    private Toggle musicToggle;

    [SerializeField]
    private Toggle soundEffectToggle;

    public static UIManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UIManager>();
                if (instance == null)
                {
                    Debug.LogError("No UIManager found in scene. Creating instance.");
                    instance = new UIManager();
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
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "Victory")
        {
            currentScoreText.text = $"YOUR SCORE:\n{ScoreManager.Instance.GetCurrentScore()}";
            bestScoreText.text = $"BEST SCORE: {ScoreManager.Instance.GetBestScore()}";
        }

        if (musicToggle != null && soundEffectToggle != null)
        {
            musicToggle.isOn = PlayerPrefs.GetInt("MusicOn", 1) == 1;
            soundEffectToggle.isOn = PlayerPrefs.GetInt("SFXOn", 1) == 1;
        }
    }

    public void OnStartButton()
    {
        SoundManager.Instance.PlaySound(audioSource, clickSound);
        if (startButton != null)
        {
            StartCoroutine(LoadSceneAfterSeconds("BattleScene", .5f));
        }
    }

    IEnumerator LoadSceneAfterSeconds(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
    }

    public void OnMenuButton()
    {
        SoundManager.Instance.PlaySound(audioSource, clickSound);
        if (menuButton != null)
        {
            StartCoroutine(LoadSceneAfterSeconds("MenuScene", .5f));
        }
    }

    public void OnSettingsPanelButton()
    {
        SoundManager.Instance.PlaySound(audioSource, clickSound);
        if (settingsMenuButton != null)
        {
            settingsPanel.SetActive(!settingsPanel.activeSelf);
        }
    }

    public void OnAboutButton()
    {
        SoundManager.Instance.PlaySound(audioSource, clickSound);
        if (aboutButton != null)
        {
            StartCoroutine(LoadSceneAfterSeconds("About", .5f));
        }
    }

    public void OnAttackButton()
    {
        SoundManager.Instance.PlaySound(audioSource, attackSound);
    }

    public void OnScrambleButton()
    {
        SoundManager.Instance.PlaySound(audioSource, scrambleSound);
    }

    public void OnMusicToggle()
    {
        SoundManager.Instance.ToggleMusic(musicToggle.isOn);
    }

    public void OnSoundEffectToggle()
    {
        SoundManager.Instance.ToggleSFX(soundEffectToggle.isOn);
    }

    public void OnBackButton()
    {
        SoundManager.Instance.PlaySound(audioSource, clickSound);
        if (backButton != null)
        {
            StartCoroutine(LoadSceneAfterSeconds("MenuScene", .5f));
        }
    }
}
