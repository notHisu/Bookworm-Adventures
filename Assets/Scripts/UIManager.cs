using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // Singleton instance of the UIManager
    private static UIManager instance;

    // Buttons in the UI
    [SerializeField]
    private Button startButton, // Button to start the game
        menuButton, // Button to open the menu
        aboutButton, // Button to open the about section
        settingsMenuButton, // Button to open the settings menu
        quitButton, // Button to quit the game
        backButton, // Button to go back from a menu
        scrambleButton, // Button to scramble letters in the game
        attackButton; // Button to attack in the game

    // Sound to play when a button is clicked
    [SerializeField]
    private AudioClip clickSound;

    // AudioSource to play sounds
    [SerializeField]
    private AudioSource audioSource;

    // Sounds to play when scramble and attack actions are performed
    [SerializeField]
    private AudioClip scrambleSound;

    [SerializeField]
    private AudioClip attackSound;

    // Text elements to display the current and best scores
    [SerializeField]
    private TMP_Text currentScoreText,
        bestScoreText;

    // Panel for the settings menu
    [SerializeField]
    private GameObject settingsPanel;

    // Panel for the about menu
    [SerializeField]
    private GameObject aboutPanel;

    // Toggles for enabling/disabling music and sound effects
    [SerializeField]
    private Toggle musicToggle;

    [SerializeField]
    private Toggle soundEffectToggle;

    // Singleton instance of the UIManager
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

    // Method called on the frame when a script is enabled just before any of the Update methods are called the first time
    private void Start()
    {
        // Check if the active scene is the "Victory" scene
        if (SceneManager.GetActiveScene().name == "Victory")
        {
            // If it is, set the current score and best score texts to the current and best scores from the ScoreManager
            currentScoreText.text = $"YOUR SCORE:\n{ScoreManager.Instance.GetCurrentScore()}";
            bestScoreText.text = $"BEST SCORE: {ScoreManager.Instance.GetBestScore()}";
        }

        // Check if the music and sound effect toggles are not null
        if (musicToggle != null && soundEffectToggle != null)
        {
            // If they are not, set their states based on the "MusicOn" and "SFXOn" values in PlayerPrefs
            // PlayerPrefs is a way to save and load data between game sessions
            // If "MusicOn" or "SFXOn" is not set in PlayerPrefs, default to 1 (on)
            musicToggle.isOn = PlayerPrefs.GetInt("MusicOn", 1) == 1;
            soundEffectToggle.isOn = PlayerPrefs.GetInt("SFXOn", 1) == 1;
        }
    }

    // Method called when the start button is clicked
    public void OnStartButton()
    {
        // Play the click sound using the SoundManager
        SoundManager.Instance.PlaySound(audioSource, clickSound);

        // Check if the start button is not null
        if (startButton != null)
        {
            // If it is not, start a coroutine to load the "BattleScene" after a delay of 0.5 seconds
            StartCoroutine(LoadSceneAfterSeconds("BattleScene", .5f));
        }
    }

    // Coroutine to load a scene after a specified delay
    IEnumerator LoadSceneAfterSeconds(string sceneName, float delay)
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);
        // Load the specified scene
        SceneManager.LoadScene(sceneName);
    }

    // Method called when the menu button is clicked
    public void OnMenuButton()
    {
        // Play the click sound
        SoundManager.Instance.PlaySound(audioSource, clickSound);
        // If the menu button is not null, load the "MenuScene" after a delay of 0.5 seconds
        if (menuButton != null)
        {
            StartCoroutine(LoadSceneAfterSeconds("MenuScene", .5f));
        }
    }

    // Method called when the settings panel button is clicked
    public void OnSettingsPanelButton()
    {
        // Play the click sound
        SoundManager.Instance.PlaySound(audioSource, clickSound);
        // If the settings menu button is not null, toggle the active state of the settings panel
        if (settingsMenuButton != null)
        {
            settingsPanel.SetActive(!settingsPanel.activeSelf);
        }
    }

    // Method called when the about button is clicked
    public void OnAboutButton()
    {
        // Play the click sound
        SoundManager.Instance.PlaySound(audioSource, clickSound);
        // If the about button is not null, load the "About" scene after a delay of 0.5 seconds
        if (aboutButton != null)
        {
            aboutPanel.SetActive(!aboutPanel.activeSelf);
        }
    }

    // Method called when the attack button is clicked
    public void OnAttackButton()
    {
        // Play the attack sound
        SoundManager.Instance.PlaySound(audioSource, attackSound);
    }

    // Method called when the scramble button is clicked
    public void OnScrambleButton()
    {
        // Play the scramble sound
        SoundManager.Instance.PlaySound(audioSource, scrambleSound);
    }

    // Method called when the music toggle is clicked
    public void OnMusicToggle()
    {
        // Toggle the music based on the state of the music toggle
        SoundManager.Instance.ToggleMusic(musicToggle.isOn);
    }

    // Method called when the sound effect toggle is clicked
    public void OnSoundEffectToggle()
    {
        // Toggle the sound effects based on the state of the sound effect toggle
        SoundManager.Instance.ToggleSFX(soundEffectToggle.isOn);
    }

    // Method called when the back button is clicked
    public void OnBackButton()
    {
        // Play the click sound
        SoundManager.Instance.PlaySound(audioSource, clickSound);
        // If the back button is not null, load the "MenuScene" after a delay of 0.5 seconds
        if (backButton != null)
        {
            StartCoroutine(LoadSceneAfterSeconds("MenuScene", .5f));
        }
    }

    // Method called when the quit button is clicked
    public void OnQuitButton()
    {
        // Play the click sound
        SoundManager.Instance.PlaySound(audioSource, clickSound);
        // If the quit button is not null, quit the application
        if (quitButton != null)
        {
            Application.Quit();
        }
    }
}
