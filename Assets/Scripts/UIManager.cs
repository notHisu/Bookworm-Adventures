using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public Button startButton;
    public Button menuButton;
    public AudioSource buttonMusic;
    
    public static UIManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UIManager>();
                if (instance == null)
                {
                    Debug.LogError("No LetterGrid found in scene. Creating instance.");
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
        
    public void  OnStartButton()
    {
        if(startButton != null)
        {
            buttonMusic.Play();
            StartCoroutine(LoadSceneAfterSeconds("BattleScene", .5f));
        }
    }
    IEnumerator LoadSceneAfterSeconds(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);

    }
    public void OnSettingsButton()
    {
        buttonMusic.Play();

    }

    public void OnMenuButton()
    {
        if(menuButton != null)
        {
            buttonMusic.Play();
            StartCoroutine(LoadSceneAfterSeconds("MenuScene", .5f));
        }
    }
}
