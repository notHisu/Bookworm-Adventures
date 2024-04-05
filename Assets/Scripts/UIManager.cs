using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
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
        settingstButton,
        scrambleButton,
        attackButton;

    [SerializeField]
    private AudioClip clickSound;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip scrambleSound;
    [SerializeField] private AudioClip attackSound;


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

    public void OnAboutButton()
    {
        SoundManager.Instance.PlaySound(audioSource, clickSound);
        if (aboutButton != null)
        {
            StartCoroutine(LoadSceneAfterSeconds("About", .5f));
        }
    }

    public void OnSettingsButton()
    {
        SoundManager.Instance.PlaySound(audioSource, clickSound);
        if (settingstButton != null)
        {
            StartCoroutine(LoadSceneAfterSeconds("Settings", .5f));
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
}
