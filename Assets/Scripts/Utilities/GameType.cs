using UnityEngine;

namespace GameTypes
{
    #region Enums
    public enum GameState
    {
        MainMenu,
        Playing,
        Paused,
        GameOver,
        LevelUp,
        Shopping,
        Loading
    }

    public enum EnemyType
    {
        Basic,
        FastMelee,
        RangedAttacker,
        Tank,
        Boss,
        Swarmer,
        Elite,
        Healer,
        Summoner
    }

    public enum PickupType
    {
        HealthOrb,
        ManaOrb,
        ExperienceGem,
        CryptoGem,
        WeaponUpgrade,
        TemporaryBuff,
        Gold,
        SpecialItem
    }
    #endregion

    #region Data Structures
    [System.Serializable]
    public class WeaponData
    {
        public string weaponId;
        public int level;
        public float damageModifier;
        public float speedModifier;
        public float rangeModifier;
        public bool isUnlocked;

        public WeaponData(WeaponBase weapon)
        {
            this.weaponId = weapon.GetType().Name;
            this.level = weapon.Level;
            this.damageModifier = weapon.DamageModifier;
            this.speedModifier = weapon.SpeedModifier;
            this.rangeModifier = weapon.RangeModifier;
            this.isUnlocked = true;
        }
    }
    #endregion
}