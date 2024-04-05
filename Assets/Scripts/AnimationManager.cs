using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    private static AnimationManager instance;
    public static AnimationManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<AnimationManager>();
                if (instance == null)
                {
                    Debug.LogError("No AnimationManager found in scene. Creating instance.");
                    instance = new AnimationManager();
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

    public void PlayAnimation(Animator animator, string animationName)
    {
        animator.Play(animationName);
    }
}
