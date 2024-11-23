using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    [SerializeField] protected GameObject player;
    protected SpriteRenderer spriteRenderer;

    [Header("Base Stats")]
    protected float health;
    protected float damage;
    protected float moveSpeed;
    protected int gemDropAmount;
    protected int experienceValue;

    protected virtual void Start()
    {
        // If player reference is not set, try to find it
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public abstract void Move();
    protected void FacePlayer()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.flipX = transform.position.x > player.transform.position.x;
        }
    }
    protected void FaceBack()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.flipX = transform.position.x < player.transform.position.x;
        }
    }
    public abstract void Attack();

    public abstract void TakeDamage(float damage);

    protected virtual void OnDeath()
    {
        Debug.Log("enemy died");
        // Drop resources
        // Spawn effects
        // Update player statistics
    }
}