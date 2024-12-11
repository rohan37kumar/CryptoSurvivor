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

    [Header("Pause Menu")]
    [SerializeField] private GameObject pauseMenuUI;

    [Header("Session Stats")]
    private int goldCollectedThisSession;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        goldCollectedThisSession = 0;
        pauseMenuUI.SetActive(false);
    }

    public void TogglePause()
    {
        if (currentState != GameState.Playing && currentState != GameState.Paused)
            return;

        isGamePaused = !isGamePaused;

        if (isGamePaused)
        {
            Time.timeScale = 0f;
            currentState = GameState.Paused;
            if (pauseMenuUI != null) pauseMenuUI.SetActive(true);
        }
        else
        {
            Time.timeScale = 1f;
            currentState = GameState.Playing;
            if (pauseMenuUI != null) pauseMenuUI.SetActive(false);
        }
    }

    public void AddSessionGold(int amount)
    {
        goldCollectedThisSession += amount;
    }

    public void EndGame()
    {
        if (AppManager.Instance != null)
        {
            AppManager.Instance.AddGold(goldCollectedThisSession);
        }

        goldCollectedThisSession = 0;

        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }

}
