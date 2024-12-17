using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManagerMainMenu : MonoBehaviour
{
    public static UIManagerMainMenu Instance { get; private set; }

    [SerializeField] private AppManager appManager;

    [Header("UI Panels")]
    public GameObject pauseMenu;
    public GameObject buyPanel;
    
    [Header("UI Elements")]
    [SerializeField] private Text goldText;
    [SerializeField] private Text energyText;
    [SerializeField] private Text energyTimerText;
    public Text tokenBalance;

    [Header("Buttons")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button buyEnergyButton;
    [SerializeField] private Button buy1EnergyButton;
    [SerializeField] private Button buy3EnergyButton;
    [SerializeField] private Button buyFullEnergyButton;
    [SerializeField] private Button crossButton;
    

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        SetupButtons();
    }

    private void Start()
    {
        SetupButtons();
    }

    private void SetupButtons()
    {
        if (playButton != null)
        {
            playButton.onClick.RemoveAllListeners();
            playButton.onClick.AddListener(() => {
                if (AppManager.Instance != null)
                {
                    AppManager.Instance.StartGame();
                }
            });
        }

        if (buyEnergyButton != null)
        {
            buyEnergyButton.onClick.RemoveAllListeners();
            buyEnergyButton.onClick.AddListener(() => {
                if (AppManager.Instance != null)
                {
                    AppManager.Instance.PromptBuyUI();
                }
            });
        }

        if (buy1EnergyButton != null)
        {
            buy1EnergyButton.onClick.RemoveAllListeners();
            buy1EnergyButton.onClick.AddListener(() => {
                if (AppManager.Instance != null)
                {
                    AppManager.Instance.Buy1Energy();
                }
            });
        }

        if (buy3EnergyButton != null)
        {
            buy3EnergyButton.onClick.RemoveAllListeners();
            buy3EnergyButton.onClick.AddListener(() => {
                if (AppManager.Instance != null)
                {
                    AppManager.Instance.Buy3Energy();
                }
            });
        }

        if (buyFullEnergyButton != null)
        {
            buyFullEnergyButton.onClick.RemoveAllListeners();
            buyFullEnergyButton.onClick.AddListener(() => {
                if (AppManager.Instance != null)
                {
                    AppManager.Instance.BuyFullEnergy();
                }
            });
        }

        if (crossButton != null)
        {
            crossButton.onClick.RemoveAllListeners();
            crossButton.onClick.AddListener(() => {
                if (AppManager.Instance != null)
                {
                    AppManager.Instance.PromptBuyUI();
                }
            });
        }
    }

    public void UpdateResourceUI(int gold, int currentEnergy, int maxEnergy)
    {
        if (goldText != null) goldText.text = $"{gold}";
        if (energyText != null) energyText.text = $"{currentEnergy}/{maxEnergy}";
    }

    public void UpdateEnergyTimerUI(string timerText)
    {
        if (energyTimerText != null)
        {
            energyTimerText.text = timerText;
        }
    }

    public void ToggleBuyPanel()
    {
        if (buyPanel != null)
        {
            buyPanel.SetActive(!buyPanel.activeSelf);
        }
    }

}