using UnityEngine;
using UnityEngine.SceneManagement;
using GameTypes;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    [Header("Game State")]
    public float gameTime;
    public bool isGamePaused;
    public GameState currentState;
    
    [Header("Session Stats")]
    private int goldCollectedThisSession;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        
        goldCollectedThisSession = 0;
    }

    public void AddSessionGold(int amount)
    {
        goldCollectedThisSession += amount;
    }

    public void EndGame(bool playerDied)
    {
        // Save collected gold to persistent storage
        if (AppManager.Instance != null)
        {
            AppManager.Instance.AddGold(goldCollectedThisSession);
        }

        // Return to main menu
        SceneManager.LoadScene("MainMenu");
    }
}
