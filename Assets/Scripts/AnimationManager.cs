using UnityEngine;

// Singleton class to manage animations
public class AnimationManager : MonoBehaviour
{
    // Private static instance of the AnimationManager
    private static AnimationManager instance;

    // Public static property to access the instance of the AnimationManager
    public static AnimationManager Instance
    {
        get
        {
            // If the instance is null, try to find an AnimationManager in the scene
            if (instance == null)
            {
                instance = FindObjectOfType<AnimationManager>();

                // If no AnimationManager was found in the scene, log an error and create a new instance
                if (instance == null)
                {
                    Debug.LogError("No AnimationManager found in scene. Creating instance.");
                    instance = new AnimationManager();
                }
            }

            // Return the instance of the AnimationManager
            return instance;
        }
    }

    // Method called when the script instance is being loaded
    void Awake()
    {
        // If an instance already exists and it's not this instance, destroy this instance
        if (instance != null && instance != this)
        {
            DestroyImmediate(gameObject); // Destroy duplicate if it exists
            return;
        }

        // Set the instance to this instance
        instance = this;
    }

    // Method to play an animation
    public void PlayAnimation(Animator animator, string animationName)
    {
        // Play the animation with the given name on the given animator
        animator.Play(animationName);
    }
}
