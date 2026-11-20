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

    private float lastAttackTiem = -999f;

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
        if (Time.time - lastAttackTiem < attackCooldown) return;

        target.TakeDamage(damage, state.MyColor);
        Debug.Log("몬스터가 플레이어를 공격!");
        lastAttackTiem = Time.time;
        OnAttacked?.Invoke();
    }
}
