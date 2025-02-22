using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private List<WeaponBase> equippedWeapons = new List<WeaponBase>();
    [SerializeField] private int maxWeapons = 6;
    
    private PlayerStats stats;
    
    private void Start()
    {
        stats = GetComponent<PlayerStats>();
    }
    
    public bool EquipWeapon(WeaponBase weapon)
    {
        if (equippedWeapons.Count >= maxWeapons) return false;
        
        equippedWeapons.Add(weapon);
        weapon.Initialize(stats);
        return true;
    }
    
    public void UpgradeWeapon(int index)
    {
        if (index < equippedWeapons.Count)
            equippedWeapons[index].Upgrade();
    }
}