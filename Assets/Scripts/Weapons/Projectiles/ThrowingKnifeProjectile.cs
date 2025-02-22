using UnityEngine;

public class ThrowingKnifeProjectile : ProjectileBase
{
    [SerializeField] private float rotationSpeed = 720f;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Update()
    {
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }

    protected override void OnHit()
    {
        base.OnHit();
    }
} 