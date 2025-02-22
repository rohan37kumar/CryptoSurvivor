using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Core Stats")]
    public float strength;      // Affects physical weapon damage
    public float arcane;        // Affects magical weapon damage
    public float agility;       // Affects movement speed
    public float endurance;     // Affects max health
    public float sense;         // Reserved for future mechanics

    [Header("Derived Stats")]
    public float maxHealth;
    public float currentHealth;
    public float moveSpeedModifier;

    [Header("Stat Scaling")]
    [SerializeField] private float healthPerEndurance = 10f;
    [SerializeField] private float speedPerAgility = 0.05f;
    [SerializeField] private float baseHealth = 100f;
    [SerializeField] private float baseSpeed = 1f;

    private void Start()
    {
        UpdateDerivedStats();
    }

    public void UpdateDerivedStats()
    {
        // Calculate max health
        maxHealth = baseHealth + (endurance * healthPerEndurance);
        if (currentHealth <= 0) currentHealth = maxHealth;

        // Calculate move speed modifier
        moveSpeedModifier = baseSpeed + (agility * speedPerAgility);
    }

    public float GetWeaponDamageMultiplier(WeaponType weaponType)
    {
        return weaponType == WeaponType.Strength ? (1f + (strength * 0.1f)) : (1f + (arcane * 0.1f));
    }
}