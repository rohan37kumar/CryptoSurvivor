using UnityEngine;

public class BOSSEnemy : EnemyBase
{
    [Header("Boss Settings")]
    [SerializeField] private float baseSpeed = 2f;
    [SerializeField] private float healthAmount = 300f;
    [SerializeField] private float damageAmount = 50f;

    [Header("Attack Range")]
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float attackCooldown = 1.5f;

    private Animator animator;
    private float attackTimer;

    protected override void Start()
    {
        base.Start();

        // Initialize stats
        health = healthAmount;
        damage = damageAmount;
        moveSpeed = baseSpeed;

        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Move();
        FacePlayer();
        if (IsPlayerInRange())
        {
            Attack();
        }
    }

    public override void Move()
    {
        if (player == null || IsPlayerInRange()) return;

        // Move directly towards the player
        Vector2 direction = (player.transform.position - transform.position).normalized;
        transform.position += (Vector3)(direction * moveSpeed * Time.deltaTime);

        if (animator != null)
        {
            animator.SetBool("isAtk", false);
        }
    }

    public override void Attack()
    {
        if (attackTimer <= 0)
        {
            if (animator != null)
            {
                animator.SetBool("isAtk", true);
            }
            //Debug.Log("Boss attacks the player!");
            playerController.TakeDamage(20.0f);

            attackTimer = attackCooldown;
        }
        else
        {
            attackTimer -= Time.deltaTime;
        }
    }

    private bool IsPlayerInRange()
    {
        return Vector2.Distance(transform.position, player.transform.position) <= attackRange;
    }

    public override void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            OnDeath();
        }
    }
}
