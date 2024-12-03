using UnityEngine;

public class GhostEnemy : EnemyBase
{
    [Header("Ghost Movement Settings")]
    [SerializeField] private float baseSpeed = 3f;
    [SerializeField] private float smoothTime = 0.5f;

    [Header("Ghost Behavior")]
    [SerializeField] private float healthAmount = 50f;
    [SerializeField] private float damageAmount = 10f;

    private Vector2 currentVelocity;

    protected override void Start()
    {
        base.Start();

        // Initialize stats
        health = healthAmount;
        damage = damageAmount;
        moveSpeed = baseSpeed;
    }

    private void Update()
    {
        Move();
        FacePlayer();
    }

    public override void Move()
    {
        if (player == null) return;

        Vector2 currentPosition = transform.position;
        Vector2 targetPosition = player.transform.position;

        // Use SmoothDamp for ghost-like floating movement
        Vector2 newPosition = Vector2.SmoothDamp(
            currentPosition,
            targetPosition,
            ref currentVelocity,
            smoothTime,
            moveSpeed
        );

        transform.position = newPosition;
    }



    public override void Attack()
    {
        // Called when colliding with player
        // Implementation depends on your damage system
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