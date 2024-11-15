using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    private float damage;
    private bool hasHit;
    
    public void Initialize(float damage)
    {
        this.damage = damage;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasHit) return;
        
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyBase>().TakeDamage(damage);
            hasHit = true;
            Destroy(gameObject);
        }
    }
}