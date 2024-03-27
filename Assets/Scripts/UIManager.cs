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

    public void OnStartButton()
    {
        if(startButton != null)
        {
            SceneManager.LoadScene("BattleScene");
        }
    }

    public void OnMenuButton()
    {
        if(menuButton != null)
        {
            SceneManager.LoadScene("MenuScene");
        }
    }
}
