using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    [Header("Base Stats")]
    protected float health;
    protected float damage;
    protected float moveSpeed;
    protected int gemDropAmount;
    protected int experienceValue;
    
    public abstract void Move();
    public abstract void Attack();

    public abstract void TakeDamage(float damage);
    
    protected virtual void OnDeath()
    {
        // Drop resources
        // Spawn effects
        // Update player statistics
    }
}