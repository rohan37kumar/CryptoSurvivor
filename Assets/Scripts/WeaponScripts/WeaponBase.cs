using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    [Header("Weapon Stats")]
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
        return baseDamage * DamageModifier * level;
    }
    
    protected float GetCurrentAttackSpeed()
    {
        return baseAttackSpeed * SpeedModifier;
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