using UnityEngine;
using UnityEngine.SceneManagement;
using GameTypes;
using System.Collections;

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
        Time.timeScale = 1f;
        
        // Load MainMenu scene
        StartCoroutine(LoadMainMenuScene());
    }

    private IEnumerator LoadMainMenuScene()
    {
        // Load the scene asynchronously
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("MainMenu");
        
        // Wait until the scene is fully loaded
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }

    private void OnDestroy()
    {
        // Perform any necessary cleanup here
        if (isGamePaused)
        {
            Time.timeScale = 1f;
        }
    }

}
