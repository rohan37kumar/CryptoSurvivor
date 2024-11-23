using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float baseSpeed = 5f;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private PlayerStats stats;
    [SerializeField] private GameObject playerImage;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Vector2 moveDirection;

    [Header("Attack Settings")]
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float attackInterval = 1f;
    private void Start()
    {
        spriteRenderer = playerImage.GetComponent<SpriteRenderer>();
        animator = playerImage.GetComponent<Animator>();

        StartCoroutine(ContinuousAttack());
    }

    private void Update()
    {
        moveDirection = InputManager.Instance.SwipeInput;
        UpdateAnimation();
    }

    private void FixedUpdate()
    {
        Move();
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
            if (moveDirection != Vector2.zero)
            {
                animator.SetBool("isMoving", true);
                //Image flipping code here...
                if (spriteRenderer != null)
                {
                    if (moveDirection.x < 0) // Moving left
                    {
                        spriteRenderer.flipX = true;
                    }
                    else if (moveDirection.x > 0) // Moving right
                    {
                        spriteRenderer.flipX = false;
                    }
                }

            }
            else
            {
                animator.SetBool("isMoving", false);
            }
        }
    }

    private IEnumerator ContinuousAttack()
    {
        while (true)
        {
            // Trigger attack animation
            animator.SetBool("playerAttacks", true);

            // Damage enemies in the horizontal direction
            DealDamageToEnemies();

            // Wait for the next attack interval
            yield return new WaitForSeconds(attackInterval);

            animator.SetBool("playerAttacks", false); // Reset attack animation
        }
    }

    private void DealDamageToEnemies()
    {
        Vector2 attackDirection = spriteRenderer.flipX ? Vector2.left : Vector2.right;
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, attackDirection, attackRange);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                // Attempt to get the EnemyBase component
                EnemyBase enemy = hit.collider.GetComponent<EnemyBase>();
                if (enemy != null)
                {
                    float damage = stats.baseStrength + stats.strengthLevel * 2;
                    enemy.TakeDamage(damage);
                }
            }
        }
    }

}
