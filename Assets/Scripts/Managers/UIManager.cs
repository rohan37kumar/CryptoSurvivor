using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject mainMenu;
    public GameObject gameHUD;
    public GameObject pauseMenu;
    public GameObject upgradePanel;
    public GameObject statsPanel;
    public GameObject cryptoPanel;
    
    [Header("HUD Elements")]
    public Slider healthBar;
    public Slider manaBar;
    public Slider staminaBar;
    public Text gemCounter;
    public Text tokenBalance;
    
    public void UpdateHUD()
    {
        // Update all UI elements
    }
}