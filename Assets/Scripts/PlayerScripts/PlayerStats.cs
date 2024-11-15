using UnityEngine;
public class PlayerStats : MonoBehaviour
{
    [Header("Base Stats")]
    public float baseHealth;
    public float baseMana;
    public float baseStamina;
    public float baseStrength;
    public float baseDexterity;
    public float baseResistance;
    public float baseWisdom;
    public float baseAgility;
    public float baseLuck;

    [Header("Leveled Stats")]
    public float currentHealth;
    public float currentMana;
    public float currentStamina;
    public int strengthLevel;
    public int dexterityLevel;
    public int resistanceLevel;
    public int wisdomLevel;
    public int agilityLevel;
    public int agilityModifier;
    public int luckLevel;
    
    [Header("Resources")]
    public int gems;
    public int experience;
    public int level;
    public float cryptoTokens;

}