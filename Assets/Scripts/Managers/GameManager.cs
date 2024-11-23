using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameTypes;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    [Header("Game State")]
    public float gameTime;
    public bool isGamePaused;

    public GameState currentState;
    
    /*
    [Header("Crypto Integration")]
    public int activeDailyPlayers;
    public float tokenDistributionThreshold;
    public float tokenPool;
    */

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    
}