using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;

public class AppManager : MonoBehaviour
{
    public static AppManager Instance { get; private set; }

    [Header("Player Resources")]
    public int CurrentGold;
    public int CurrentEnergy;

    [Header("Energy System")]
    private const int MAX_ENERGY = 10;
    private const int ENERGY_COST_PER_PLAY = 1;
    private const int MINUTES_PER_ENERGY = 60;
    private DateTime lastEnergyUpdateTime;

    private SaveManager saveManager;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            // Wait for SaveManager to be initialized
            StartCoroutine(InitializeManagersWhenReady());
        }
        else
        {
            // If an instance already exists, destroy this one
            Destroy(gameObject);
        }

        //Remove this after testing
        //GetEnergyForDev();
    }

    private IEnumerator InitializeManagersWhenReady()
    {
        // Wait until SaveManager is available
        while (SaveManager.Instance == null)
        {
            yield return null;
        }
        
        InitializeManagers();
    }

    private void InitializeManagers()
    {
        LoadPlayerData();
        SetupEnergySystem();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainMenu")
        {
            Time.timeScale = 1f;
            Canvas.ForceUpdateCanvases();

            // UpdateUI();
            // ShowMainMenuUI();
            StartCoroutine(InitializeUIAfterLoad());
        }
    }

    private IEnumerator InitializeUIAfterLoad()
    {
        // Wait for a frame to ensure all objects are initialized
        yield return null;
        
        // Update UI
        UpdateUI();
        
    }

    private void UpdateUI()
    {
        if (UIManagerMainMenu.Instance != null)
        {
            UIManagerMainMenu.Instance.UpdateResourceUI(CurrentGold, CurrentEnergy, MAX_ENERGY);
            //UpdateEnergyTimer();
        }
    }

    private void UpdateEnergyTimer()
    {
        if (CurrentEnergy >= MAX_ENERGY)
        {
            UIManagerMainMenu.Instance.UpdateEnergyTimerUI("Full Energy");
            return;
        }

        TimeSpan timeUntilNextEnergy = lastEnergyUpdateTime.AddMinutes(MINUTES_PER_ENERGY) - DateTime.Now;

        if (timeUntilNextEnergy.TotalSeconds > 0)
        {
            string timerText = $"Next Energy in: {timeUntilNextEnergy.Minutes:D2}:{timeUntilNextEnergy.Seconds:D2}";
            UIManagerMainMenu.Instance.UpdateEnergyTimerUI(timerText);
        }
        else
        {
            UpdateEnergyBasedOnTime();
        }
    }

    public void PromptBuyUI()
    {
        //Debug.Log("buy UI panel display");
        if (UIManagerMainMenu.Instance != null)
        {
            UIManagerMainMenu.Instance.ToggleBuyPanel();
        }
    }

    private void SetupEnergySystem()
    {
        // Load the last energy update time
        string savedTimeStr = PlayerPrefs.GetString("LastEnergyTime", "");

        if (string.IsNullOrEmpty(savedTimeStr))
        {
            lastEnergyUpdateTime = DateTime.Now;
        }
        else
        {
            if (DateTime.TryParse(savedTimeStr, out DateTime savedTime))
            {
                lastEnergyUpdateTime = savedTime;
                UpdateEnergyBasedOnTime();
            }
            else
            {
                lastEnergyUpdateTime = DateTime.Now;
            }
        }

        // Start the update cycle
        InvokeRepeating("UpdateEnergyTimer", 0f, 1f);
    }

    private void UpdateEnergyBasedOnTime()
    {
        if (CurrentEnergy >= MAX_ENERGY)
        {
            lastEnergyUpdateTime = DateTime.Now;
            return;
        }

        TimeSpan timeSinceLastUpdate = DateTime.Now - lastEnergyUpdateTime;
        int energyToAdd = (int)(timeSinceLastUpdate.TotalMinutes / MINUTES_PER_ENERGY);

        if (energyToAdd > 0)
        {
            CurrentEnergy = Mathf.Min(MAX_ENERGY, CurrentEnergy + energyToAdd);

            // Update the last energy time to account for the energy we just added
            int minutesUsed = energyToAdd * MINUTES_PER_ENERGY;
            lastEnergyUpdateTime = lastEnergyUpdateTime.AddMinutes(minutesUsed);

            SavePlayerData();
            UpdateUI();
        }
    }

    private void RefillEnergy(int numEnergy)
    {
        CurrentEnergy += numEnergy;
        lastEnergyUpdateTime = DateTime.Now;
        SavePlayerData();
        UpdateUI();
    }

    private void GetEnergyForDev(){
        CurrentEnergy = 9;
        lastEnergyUpdateTime = DateTime.Now;
        SavePlayerData();
        UpdateUI();
    }

    private void LoadPlayerData()
    {
        // Add null check
        if (SaveManager.Instance == null || SaveManager.Instance.CurrentData == null)
        {
            Debug.LogWarning("SaveManager not initialized when loading player data");
            return;
        }
        
        var saveData = SaveManager.Instance.CurrentData;
        CurrentGold = saveData.gold;
        CurrentEnergy = saveData.energy;
        
        // Load the last energy update time
        if (DateTime.TryParse(PlayerPrefs.GetString("LastEnergyTime", ""), out DateTime savedTime))
        {
            lastEnergyUpdateTime = savedTime;
            UpdateEnergyBasedOnTime();
        }
        else
        {
            lastEnergyUpdateTime = DateTime.Now;
        }
    }

    private void SavePlayerData()
    {
        SaveManager.Instance.UpdateResources(CurrentGold, CurrentEnergy);
        PlayerPrefs.SetString("LastEnergyTime", lastEnergyUpdateTime.ToString("o"));
        PlayerPrefs.Save();
    }

    public void Buy1Energy() => BuyEnergy(1);
    public void Buy3Energy() => BuyEnergy(3);
    public void BuyFullEnergy()
    {
        int energyNeeded = MAX_ENERGY - CurrentEnergy;
        BuyEnergy(energyNeeded); // Ensure it refills to max
    }

    public void BuyEnergy(int numEnergy)
    {
        Debug.Log("initiating payment gateway...");

        int energyToAdd = Mathf.Min(numEnergy, MAX_ENERGY - CurrentEnergy);

        //buy the energies in some payment manager
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            SavePlayerData();
        }
        else
        {
            UpdateEnergyBasedOnTime();
        }
    }
    private void OnApplicationQuit()
    {
        SavePlayerData();
    }

    public void AddGold(int amount)
    {
        CurrentGold += amount;
        SavePlayerData();
        UpdateUI();
    }

    public void StartGame()
    {
        if (CurrentEnergy >= ENERGY_COST_PER_PLAY)
        {
            CurrentEnergy -= ENERGY_COST_PER_PLAY;
            SavePlayerData();
            SceneManager.LoadSceneAsync("PlayScene");
        }
        else
        {
            Debug.Log("Not enough energy!");
        }
    }
}
//code ends here
