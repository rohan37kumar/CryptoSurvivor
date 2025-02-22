using UnityEngine;

[System.Serializable]
public class WeaponData
{
    public string weaponId;
    public WeaponType weaponType;
    public int level;
    public float damageModifier;
    public float speedModifier;
    public float rangeModifier;
    public bool isUnlocked;

    public WeaponData(WeaponBase weapon)
    {
        this.weaponId = weapon.GetType().Name;
        this.weaponType = weapon.weaponType;
        this.level = weapon.Level;
        this.damageModifier = weapon.DamageModifier;
        this.speedModifier = weapon.SpeedModifier;
        this.rangeModifier = weapon.RangeModifier;
        this.isUnlocked = true;
    }
} 