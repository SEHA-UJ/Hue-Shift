using System;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [Header("공격")]
    [SerializeField] private EnemyState state;
    
    [Header("공격")]
    [SerializeField] private int damage = 1;
    [SerializeField] private float attackCooldown = 2f;
    
    private IDamageable target;

    private float lastAttackTime = -999f;

    public event Action OnAttacked;

    void OnTriggerEnter(Collider other)
    {
        IDamageable damageable = other.GetComponentInParent<IDamageable>();
        if (damageable != null)
            target = damageable;
    }

    void OnTriggerExit(Collider other)
    {
        IDamageable damageable = other.GetComponentInParent<IDamageable>();
        if (damageable != null && damageable == target)
            target = null;
    }

    void Update()
    {
        if (target == null) return;
        if (state.IsDead || !GameFlow.IsPlaying) return;
        if (Time.time - lastAttackTime < attackCooldown) return;

        target.TakeDamage(damage, state.MyColor);
        lastAttackTime = Time.time;
        OnAttacked?.Invoke();
    }
}
