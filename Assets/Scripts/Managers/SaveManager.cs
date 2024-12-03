using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Text;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }

    [Serializable]
    public class PlayerSaveData
    {
        public int gold;
        public int energy;
        public DateTime lastSaveTime;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SavePlayerData(int gold, int energy)
    {
        try
        {
            PlayerSaveData saveData = new PlayerSaveData
            {
                gold = gold,
                energy = energy,
                lastSaveTime = DateTime.Now
            };

            string json = JsonUtility.ToJson(saveData);
            string encodedJson = Convert.ToBase64String(Encoding.UTF8.GetBytes(json));

            // Save to both PlayerPrefs and IndexedDB
            PlayerPrefs.SetString("SaveData", encodedJson);
            PlayerPrefs.Save();

            // Additional WebGL-specific save using IndexedDB
            SaveToIndexedDB(encodedJson);
        }
        catch (Exception e)
        {
            Debug.LogError($"Error saving data: {e.Message}");
        }
    }

    public PlayerSaveData LoadPlayerData()
    {
        try
        {
            // Try to load from IndexedDB first
            string encodedJson = LoadFromIndexedDB();

            // If not found in IndexedDB, try PlayerPrefs
            if (string.IsNullOrEmpty(encodedJson))
            {
                encodedJson = PlayerPrefs.GetString("SaveData", "");
            }

            if (!string.IsNullOrEmpty(encodedJson))
            {
                string json = Encoding.UTF8.GetString(Convert.FromBase64String(encodedJson));
                return JsonUtility.FromJson<PlayerSaveData>(json);
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error loading data: {e.Message}");
        }

        // Return default data if loading fails
        return new PlayerSaveData
        {
            gold = 0,
            energy = 10,
            lastSaveTime = DateTime.Now
        };
    }

    private void SaveToIndexedDB(string data)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        SaveToIndexedDBJS(data);
#endif
    }

    private string LoadFromIndexedDB()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        return LoadFromIndexedDBJS();
#else
        return "";
#endif
    }

    // JavaScript plugin interface
    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern void SaveToIndexedDBJS(string data);

    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern string LoadFromIndexedDBJS();
}