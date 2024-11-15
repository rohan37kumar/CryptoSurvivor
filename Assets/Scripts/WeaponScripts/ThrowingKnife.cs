using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingKnife : WeaponBase
{
    [SerializeField] private GameObject knifePrefab;
    [SerializeField] private float projectileSpeed = 10f;
    [SerializeField] private int baseProjectileCount = 1;
    
    protected override void Attack()
    {
        int projectileCount = baseProjectileCount + (level / 3);
        float angleStep = 360f / projectileCount;
        
        for (int i = 0; i < projectileCount; i++)
        {
            float angle = i * angleStep;
            Vector2 direction = Quaternion.Euler(0, 0, angle) * Vector2.right;
            
            GameObject knife = Instantiate(knifePrefab, transform.position, Quaternion.Euler(0, 0, angle));
            knife.GetComponent<Rigidbody2D>().velocity = direction * projectileSpeed;
            knife.GetComponent<ProjectileScript>().Initialize(GetCurrentDamage());
            
            Destroy(knife, 2f);
        }
    }
    
    public override void Upgrade()
    {
        level++;
        baseDamage *= 1.2f;
        baseAttackSpeed *= 0.9f;
    }
}