using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    [Header("Weapon Properties")]
    public WeaponType weaponType;
    public float baseDamage;
    public float baseAttackSpeed;
    public float baseRange;
    public int level = 1;

    protected PlayerStats playerStats;
    
    // Added properties to match WeaponData structure
    public int Level => level;
    public float DamageModifier { get; protected set; } = 1f;
    public float SpeedModifier { get; protected set; } = 1f;
    public float RangeModifier { get; protected set; } = 1f;

    
    public virtual void Initialize(PlayerStats stats)
    {
        playerStats = stats;
    }
    
    
    protected abstract void Attack();
    public abstract void Upgrade();
    
    
    protected float GetCurrentDamage()
    {
        float statMultiplier = playerStats ? playerStats.GetWeaponDamageMultiplier(weaponType) : 1f;
        return baseDamage * DamageModifier * level * statMultiplier;
    }
    
    protected float GetCurrentAttackSpeed()
    {
        // Higher values now mean faster attacks
        float baseAttacksPerSecond = 1f / baseAttackSpeed;
        float agilityBonus = playerStats ? (playerStats.agility * 0.05f) : 0; // 5% faster per agility level
        
        // Apply all speed modifiers
        float totalSpeedMultiplier = (1 + agilityBonus) * SpeedModifier;
        float attacksPerSecond = baseAttacksPerSecond * totalSpeedMultiplier;
        
        // Convert back to seconds between attacks
        return 1f / attacksPerSecond;
    }
    
    protected float GetCurrentRange()
    {
        return baseRange * RangeModifier;
    }
    
    public virtual void LoadFromData(GameTypes.WeaponData data)
    {
        level = data.level;
        DamageModifier = data.damageModifier;
        SpeedModifier = data.speedModifier;
        RangeModifier = data.rangeModifier;
    }
    
    public virtual GameTypes.WeaponData CreateSaveData()
    {
        return new GameTypes.WeaponData(this);
    }
}