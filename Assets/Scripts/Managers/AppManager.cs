using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class AppManager : MonoBehaviour
{
    public static AppManager Instance { get; private set; }

    [Header("Player Resources")]
    public int CurrentGold;
    public int CurrentEnergy;

    [Header("UI References")]
    [SerializeField] private Text goldText;
    [SerializeField] private Text energyText;
    [SerializeField] private GameObject buyPanel;
    private const string PLAY_SCENE_NAME = "PlayScene";

    [Header("Energy System")]
    private const int MAX_ENERGY = 10;
    private const int ENERGY_COST_PER_PLAY = 1;
    private const int MINUTES_PER_ENERGY = 60; // One energy per hour
    private DateTime lastEnergyUpdateTime;
    [SerializeField] private Text energyTimerText;


    private SaveManager saveManager;
    private AEONPaymentManager paymentManager;


    [Header("Audio Settings")]
    [SerializeField] private AudioClip bkgMusic;
    [SerializeField] private float musicVolume = 0.5f;
    private AudioSource musicSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            /*
            if (FindObjectOfType<SaveManager>() == null)
            {
                new GameObject("SaveManager").AddComponent<SaveManager>();
            }
            saveManager = SaveManager.Instance;
            */
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        paymentManager = this.gameObject.GetComponent<AEONPaymentManager>();
        buyPanel.SetActive(false);

        LoadPlayerData();
        SetupEnergySystem();
        SetupSound();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        UpdateUI();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainMenu")
        {
            // Reconnect UI references as they're destroyed on scene load
            // goldText = GameObject.Find("GoldText").GetComponent<Text>();
            // energyText = GameObject.Find("EnergyText").GetComponent<Text>();
            // energyTimerText = GameObject.Find("EnergyTimerText").GetComponent<Text>();
            UpdateUI();
        }
    }

    public void StartGame()
    {
        Debug.Log("Starting game...");
        if (CurrentEnergy >= ENERGY_COST_PER_PLAY)
        {
            CurrentEnergy -= ENERGY_COST_PER_PLAY;
            SavePlayerData();
            UpdateUI();
            SceneManager.LoadScene(PLAY_SCENE_NAME);
        }
        else
        {
            Debug.Log("Not enough energy!");
            // Show UI notification to player
        }
    }

    public void AddGold(int amount)
    {
        CurrentGold += amount;
        SavePlayerData();
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (goldText != null) goldText.text = $"{CurrentGold}";
        if (energyText != null) energyText.text = $"{CurrentEnergy}/{MAX_ENERGY}";
        UpdateEnergyTimer();
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

    private void UpdateEnergyTimer()
    {
        if (CurrentEnergy >= MAX_ENERGY)
        {
            if (energyTimerText != null)
            {
                energyTimerText.text = "Full Energy";
            }
            return;
        }

        TimeSpan timeUntilNextEnergy = lastEnergyUpdateTime.AddMinutes(MINUTES_PER_ENERGY) - DateTime.Now;

        if (timeUntilNextEnergy.TotalSeconds > 0)
        {
            string timerText = $"{timeUntilNextEnergy.Minutes:D2}:{timeUntilNextEnergy.Seconds:D2}";
            if (energyTimerText != null)
            {
                energyTimerText.text = $"Next Energy in: {timerText}";
            }
        }
        else
        {
            UpdateEnergyBasedOnTime();
        }
    }


    private void SavePlayerData()
    {
        PlayerPrefs.SetInt("Gold", CurrentGold);
        PlayerPrefs.SetInt("Energy", CurrentEnergy);
        PlayerPrefs.SetString("LastEnergyTime", lastEnergyUpdateTime.ToString("o"));  //ISO 8601 format
        PlayerPrefs.Save();
    }

    private void LoadPlayerData()
    {
        CurrentGold = PlayerPrefs.GetInt("Gold", 0);
        CurrentEnergy = PlayerPrefs.GetInt("Energy", MAX_ENERGY);
    }

    /*
    //Better Saving methods, remember to get the Instance in the Awake function.

    private void SavePlayerData()
    {
        if (saveManager != null)
        {
            saveManager.SavePlayerData(CurrentGold, CurrentEnergy);
        }
    }

    private void LoadPlayerData()
    {
        if (saveManager != null)
        {
            var saveData = saveManager.LoadPlayerData();
            CurrentGold = saveData.gold;
            CurrentEnergy = saveData.energy;
        }
    }
    */

    private void SetupSound()
    {
        musicSource = GetComponent<AudioSource>();
        if (musicSource == null)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
        }

        musicSource.clip = bkgMusic;
        musicSource.volume = musicVolume;
        musicSource.loop = true;
        musicSource.playOnAwake = false;

        if (!musicSource.isPlaying && bkgMusic != null)
        {
            musicSource.Play();
        }
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

        paymentManager.BuyXEnergy(energyToAdd);
    }

    public void PromptBuyUI()
    {
        Debug.Log("buy panel showing");
        buyPanel.SetActive(true);
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
}
//code ends here
