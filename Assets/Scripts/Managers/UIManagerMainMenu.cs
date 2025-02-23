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
    public GameObject statsPanel;
    
    [Header("Panel Toggle Buttons")]
    [SerializeField] private Button[] buyPanelToggleButtons;  // Array for buy panel buttons
    [SerializeField] private Button[] statsPanelToggleButtons;  // Array for stats panel buttons

    [Header("UI Elements")]
    [SerializeField] private Text goldText;
    [SerializeField] private Text energyText;
    [SerializeField] private Text energyTimerText;
    public Text tokenBalance;

    [Header("Stats Display")]
    [SerializeField] private Text strengthText;
    [SerializeField] private Text arcaneText;
    [SerializeField] private Text agilityText;
    [SerializeField] private Text enduranceText;
    [SerializeField] private Text senseText;

    [Header("Action Buttons")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button buy1EnergyButton;
    [SerializeField] private Button buy3EnergyButton;
    [SerializeField] private Button buyFullEnergyButton;

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
        // Setup buy panel toggle buttons
        foreach (Button button in buyPanelToggleButtons)
        {
            if (button != null)
            {
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(ToggleBuyPanel);
            }
        }

        // Setup stats panel toggle buttons
        foreach (Button button in statsPanelToggleButtons)
        {
            if (button != null)
            {
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(ToggleStatsPanel);
            }
        }

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

    public void ToggleStatsPanel()
    {
        if (statsPanel != null)
        {
            statsPanel.SetActive(!statsPanel.activeSelf);
            if (statsPanel.activeSelf)
            {
                UpdateStatsDisplay();
            }
        }
    }

    private void UpdateStatsDisplay()
    {
        if (SaveManager.Instance != null && SaveManager.Instance.CurrentData != null && SaveManager.Instance.CurrentData.stats != null)
        {
            var statsData = SaveManager.Instance.CurrentData.stats;
            if (strengthText) strengthText.text = $"Strength: {statsData.strength}";
            if (arcaneText) arcaneText.text = $"Arcane: {statsData.arcane}";
            if (agilityText) agilityText.text = $"Agility: {statsData.agility}";
            if (enduranceText) enduranceText.text = $"Endurance: {statsData.endurance}";
            if (senseText) senseText.text = $"Sense: {statsData.sense}";
        }
    }

}