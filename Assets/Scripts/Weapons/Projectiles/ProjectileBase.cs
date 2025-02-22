using UnityEngine;

public abstract class ProjectileBase : MonoBehaviour
{
    [Header("Base Properties")]
    [SerializeField] protected float speed;
    [SerializeField] protected float lifetime = 5f;
    protected float damage;
    protected bool hasHit;

    protected Rigidbody2D rb;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, lifetime);
    }

    public virtual void Initialize(float damage)
    {
        this.damage = damage;
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (hasHit) return;
        
        if (other.CompareTag("Enemy"))
        {
            ApplyDamage(other);
            OnHit();
        }
    }

    protected virtual void ApplyDamage(Collider2D enemy)
    {
        if (enemy.TryGetComponent<EnemyBase>(out var enemyComponent))
        {
            enemyComponent.TakeDamage(damage);
        }
    }

    protected virtual void OnHit()
    {
        hasHit = true;
        Destroy(gameObject);
    }
} 