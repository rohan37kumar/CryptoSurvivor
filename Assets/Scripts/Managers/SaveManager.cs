using UnityEngine;
using System;
using System.Collections.Generic;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }

    [Serializable]
    public class PlayerStatsData
    {
        public float strength;
        public float arcane;
        public float agility;
        public float endurance;
        public float sense;

        public PlayerStatsData() { }

        public PlayerStatsData(PlayerStats stats)
        {
            strength = stats.strength;
            arcane = stats.arcane;
            agility = stats.agility;
            endurance = stats.endurance;
            sense = stats.sense;
        }
    }

    [Serializable]
    public class PlayerSaveData
    {
        public int gold;
        public int energy;
        public DateTime lastSaveTime;
        public PlayerStatsData stats;
        public List<WeaponData> weapons;
        public int highScore;
    }

    private const string SAVE_KEY = "PlayerSaveData";
    private PlayerSaveData currentData;

    public PlayerSaveData CurrentData => currentData;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveData()
    {
        try
        {
            string json = JsonUtility.ToJson(currentData);
            PlayerPrefs.SetString(SAVE_KEY, json);
            PlayerPrefs.Save();
        }
        catch (Exception e)
        {
            Debug.LogError($"Error saving data: {e.Message}");
        }
    }

    private void LoadData()
    {
        string json = PlayerPrefs.GetString(SAVE_KEY, "");
        
        if (string.IsNullOrEmpty(json))
        {
            currentData = CreateDefaultData();
        }
        else
        {
            try
            {
                currentData = JsonUtility.FromJson<PlayerSaveData>(json);
            }
            catch
            {
                currentData = CreateDefaultData();
            }
        }
    }

    private PlayerSaveData CreateDefaultData()
    {
        return new PlayerSaveData
        {
            gold = 0,
            energy = 10,
            lastSaveTime = DateTime.Now,
            stats = new PlayerStatsData 
            {
                strength = 1,
                arcane = 1,
                agility = 1,
                endurance = 1,
                sense = 1
            },
            weapons = new List<WeaponData>(),
            highScore = 0
        };
    }

    public void UpdatePlayerStats(PlayerStats stats)
    {
        currentData.stats = new PlayerStatsData(stats);
        SaveData();
    }

    public void UpdateWeapons(List<WeaponData> weapons)
    {
        currentData.weapons = weapons;
        SaveData();
    }

    public void UpdateResources(int gold, int energy)
    {
        currentData.gold = gold;
        currentData.energy = energy;
        currentData.lastSaveTime = DateTime.Now;
        SaveData();
    }

    public void UpdateHighScore(int score)
    {
        if (score > currentData.highScore)
        {
            currentData.highScore = score;
            SaveData();
        }
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            SaveData();
        }
    }

    private void OnApplicationQuit()
    {
        SaveData();
    }

    public void SaveGameSession(int sessionGold, int sessionScore)
    {
        currentData.gold += sessionGold;
        
        if (sessionScore > currentData.highScore)
        {
            currentData.highScore = sessionScore;
        }
        
        SaveData();
    }

    public void LoadPlayerStats(PlayerStats playerStats)
    {
        if (playerStats == null)
        {
            Debug.LogWarning("Cannot load player stats: playerStats is null");
            return;
        }

        if (currentData?.stats == null)
        {
            Debug.LogWarning("No saved stats found, initializing with default values");
            currentData.stats = new PlayerStatsData();
        }

        // Copy stats from saved data
        playerStats.strength = currentData.stats.strength;
        playerStats.arcane = currentData.stats.arcane;
        playerStats.agility = currentData.stats.agility;
        playerStats.endurance = currentData.stats.endurance;
        playerStats.sense = currentData.stats.sense;
        playerStats.UpdateDerivedStats();
    }

    public void SavePlayerStats(PlayerStats playerStats)
    {
        if (playerStats == null)
        {
            Debug.LogWarning("Cannot save player stats: playerStats is null");
            return;
        }

        if (currentData.stats == null)
        {
            currentData.stats = new PlayerStatsData();
        }

        // Copy stats to save data
        currentData.stats.strength = playerStats.strength;
        currentData.stats.arcane = playerStats.arcane;
        currentData.stats.agility = playerStats.agility;
        currentData.stats.endurance = playerStats.endurance;
        currentData.stats.sense = playerStats.sense;
        SaveData();
    }
}