using UnityEngine;

public class SkeletonEnemy : EnemyBase
{
    [Header("Skeleton Settings")]
    [SerializeField] private float baseSpeed = 3.5f;
    [SerializeField] private float healthAmount = 80f;
    [SerializeField] private float damageAmount = 10f;

    private Animator animator;

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
        FaceBack();
    }

    public override void Move()
    {
        if (player == null) return;

        // Move directly towards the player
        Vector2 direction = (player.transform.position - transform.position).normalized;
        transform.position += (Vector3)(direction * moveSpeed * Time.deltaTime);
    }

    public override void Attack()
    {
        // Skeleton does damage on touch
        playerController.TakeDamage(damageAmount);
        //Debug.Log("Skeleton attacks the player!");
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
