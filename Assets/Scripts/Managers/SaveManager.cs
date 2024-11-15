using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameTypes;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }
    
    [System.Serializable]
    public class SaveData
    {
        public PlayerStats playerStats;
        public List<WeaponData> unlockedWeapons;
        public float cryptoBalance;
        // Additional save data
    }
    
    public void SaveGame()
    {
        // Serialize game state
        // Save to local storage
    }
    
    public void LoadGame()
    {
        // Load from local storage
        // Deserialize and apply game state
    }
}