using UnityEngine;

public class GameManager : MonoBehaviour
{
    GameManagerSingleton gameManagerInstance = GameManagerSingleton.Instance;

    private void Awake()
    {
        gameManagerInstance.LoadBestScore();
        DontDestroyOnLoad(this.gameObject);
    }
}
