using UnityEngine;

public class SparkWeapon : WeaponBase
{
    [SerializeField] private GameObject sparkPrefab;
    
    private float lastAttackTime;

    private void Update()
    {
        if (Time.time >= lastAttackTime + GetCurrentAttackSpeed())
        {
            Attack();
            lastAttackTime = Time.time;
        }
    }

    protected override void Attack()
    {
        GameObject spark = Instantiate(sparkPrefab, transform.position, Quaternion.identity);
        spark.GetComponent<SparkProjectile>().Initialize(GetCurrentDamage());
        
        // Destroy after maximum lifetime
        Destroy(spark, 5f);
    }

    public override void Upgrade()
    {
        level++;
        baseDamage *= 1.15f; // 15% damage increase per level
        SpeedModifier *= 1.08f; // 8% speed increase per level
    }
}