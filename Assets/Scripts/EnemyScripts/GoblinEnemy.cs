using UnityEngine;

public class GoblinEnemy : EnemyBase
{
    [Header("Goblin Settings")]
    [SerializeField] private float baseSpeed = 5f;
    [SerializeField] private float healthAmount = 30f;
    [SerializeField] private float damageAmount = 5f;

    [Header("Attack Settings")]
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float attackCooldown = 1f;

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
        if (IsPlayerInRange())
        {
            Attack();
        }
        else
        {
            Move();
        }
        FacePlayer();
    }

    public override void Move()
    {
        if (player == null) return;

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

            //Debug.Log("Goblin attacks the player!");
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
            Destroy(gameObject);
        }
    }
}
