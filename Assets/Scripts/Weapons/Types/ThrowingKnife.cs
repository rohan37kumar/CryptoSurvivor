using UnityEngine;

public class ThrowingKnife : WeaponBase
{
    [SerializeField] private GameObject knifePrefab;
    [SerializeField] private float projectileSpeed = 15f;
    [SerializeField] private int baseProjectileCount = 1;
    
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
        int projectileCount = baseProjectileCount + (level / 3);
        float angleStep = 360f / projectileCount;
        
        for (int i = 0; i < projectileCount; i++)
        {
            float angle = i * angleStep;
            Vector2 direction = Quaternion.Euler(0, 0, angle) * Vector2.right;
            
            GameObject knife = Instantiate(knifePrefab, transform.position, Quaternion.Euler(0, 0, angle));
            knife.GetComponent<Rigidbody2D>().velocity = direction * projectileSpeed;
            knife.GetComponent<ThrowingKnifeProjectile>().Initialize(GetCurrentDamage());
        }
    }
    
    public override void Upgrade()
    {
        level++;
        baseDamage *= 1.25f;
        SpeedModifier *= 1.1f;
    }
}


/*
Unity Editor Setup Required:
Create a new prefab for the ThrowingKnifeProjectile:
Create a new GameObject
Add SpriteRenderer with knife sprite
Add Rigidbody2D (set to Kinematic)
Add BoxCollider2D (set as Trigger)
Add ThrowingKnifeProjectile script
Set the speed and lifetime values
Create prefab
Update the ThrowingKnife weapon prefab:
Set weaponType to Strength
Assign the knife prefab
Set base damage higher than SparkWeapon
Adjust projectileSpeed and baseProjectileCount
Make sure both weapons are properly referenced in the WeaponController component on the player.
*/