using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private Slider healthBarSlider;
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;


    [Header("Movement Settings")]
    [SerializeField] private float baseSpeed = 5f;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private PlayerStats stats;
    [SerializeField] private GameObject playerImage;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Vector2 moveDirection;

    [Header("Attack Settings")]
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private float attackAngle = 45f;
    [SerializeField] private float closeRangeRadius = 0.3f;
    [SerializeField] private float attackInterval = 1f;
    [SerializeField] private float attackDuration = 0.5f;
    private bool isAttacking = false;
    private float attackTimer = 0f;

    private void Start()
    {
        spriteRenderer = playerImage.GetComponent<SpriteRenderer>();
        animator = playerImage.GetComponent<Animator>();

        currentHealth = maxHealth;
        UpdateHealthBar();

        // if (!isAttacking)
        // {
        //     StartCoroutine(ContinuousAttack());
        // }
    }

    private void Update()
    {
        if (!isAttacking)
        {
            moveDirection = InputManager.Instance.SwipeInput;
            HandleAttack();
        }
        UpdateAnimation();
    }

    private void FixedUpdate()
    {
        if (!isAttacking)
        {
            Move();
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void UpdateHealthBar()
    {
        if (healthBarSlider != null)
        {
            healthBarSlider.value = currentHealth / maxHealth;
        }
    }
    
    private void Die()
    {
        rb.velocity = Vector2.zero;
        this.enabled = false;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.EndGame();
        }
        Debug.Log("Player has died!!");
    }

    private void Move()
    {
        float currentSpeed = baseSpeed * (1 + stats.agilityModifier);
        rb.velocity = moveDirection.normalized * currentSpeed;
    }

    private void UpdateAnimation()
    {
        if (animator != null)
        {
            animator.SetBool("isMoving", moveDirection != Vector2.zero);

            // Flip the sprite based on movement direction
            if (moveDirection.x < 0)
                spriteRenderer.flipX = true;
            else if (moveDirection.x > 0)
                spriteRenderer.flipX = false;
        }
    }

    private void HandleAttack()
    {
        attackTimer -= Time.deltaTime;

        if (attackTimer <= 0f)
        {
            StartAttack();
            attackTimer = attackInterval; // Reset the timer for the next attack
        }
    }

    private void StartAttack()
    {
        animator.SetBool("playerAttacks", true);
        isAttacking = true;

        rb.velocity = Vector2.zero;

        // Simulate attack duration
        Invoke(nameof(EndAttack), attackDuration);
    }

    private void EndAttack()
    {
        isAttacking = false;
        animator.SetBool("playerAttacks", false);

        DealDamageToEnemies();
    }

    private void DealDamageToEnemies()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRange);

        Vector2 attackDirection = spriteRenderer.flipX ? Vector2.left : Vector2.right;

        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                Vector2 directionToEnemy = (hit.transform.position - transform.position).normalized;
                float distanceToEnemy = Vector2.Distance(transform.position, hit.transform.position);

                if (distanceToEnemy <= closeRangeRadius)
                {
                    ApplyDamage(hit);
                    continue; // Skip angle check for close enemies
                }

                float angleToEnemy = Vector2.Angle(attackDirection, directionToEnemy);
                if (angleToEnemy <= attackAngle / 2)
                {
                    ApplyDamage(hit);
                }
            }
        }
    }
    private void ApplyDamage(Collider2D enemyCollider)
    {
        EnemyBase enemy = enemyCollider.GetComponent<EnemyBase>();
        if (enemy != null)
        {
            float damage = stats.baseStrength + stats.strengthLevel * 2;
            enemy.TakeDamage(damage);
        }
    }

}
