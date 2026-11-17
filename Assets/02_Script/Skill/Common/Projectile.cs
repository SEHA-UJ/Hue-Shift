using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody rb;

    private ColorType attackColor;
    private int damage;
    private float maxDistance;
    private Vector3 spawnPoint;

    private bool isLaunched;
    private bool hasHit;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    public void Launch(Vector3 dir, ColorType color, ProjectileStats state)
    {
        attackColor = color;

        damage = state.damage;
        maxDistance = state.maxDistance;

        spawnPoint = transform.position;
        rb.linearVelocity = dir.normalized * state.speed;

        isLaunched = true;
    }

    void Update()
    {
        if (!isLaunched) return;
        if (Vector3.Distance(spawnPoint, transform.position) >= maxDistance)
            Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        // 오브젝트 삭제 직전 중복 타격 방지
        if (hasHit) return;

        if (other.isTrigger) return;

        hasHit  = true;

        IDamageable target = other.GetComponentInParent<IDamageable>();
        if (target != null)
            target.TakeDamage(damage, attackColor);

        Destroy(gameObject);
    }
}
