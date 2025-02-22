using UnityEngine;

public class SparkProjectile : ProjectileBase
{
    [SerializeField] private float homingSpeed = 5f;
    [SerializeField] private float detectionRadius = 10f;
    private Transform targetEnemy;

    protected override void Awake()
    {
        base.Awake();
        FindNearestEnemy();
    }

    private void FindNearestEnemy()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius);
        float closestDistance = float.MaxValue;

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                float distance = Vector2.Distance(transform.position, collider.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    targetEnemy = collider.transform;
                }
            }
        }
    }

    private void FixedUpdate()
    {
        Vector2 direction = Vector2.up;
        if (targetEnemy != null)
        {
            direction = (targetEnemy.position - transform.position).normalized;
            rb.velocity = direction * homingSpeed;

            // Update rotation to face movement direction
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
        else
        {
            //Debug.Log("no enemy found");
            direction = Vector2.up;
            rb.velocity = direction * homingSpeed;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}