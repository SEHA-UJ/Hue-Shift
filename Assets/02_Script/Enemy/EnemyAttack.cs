using System;
using System.Collections;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [Header("공격")]
    [SerializeField] private EnemyState state;
    
    [Header("공격")]
    [SerializeField] private int damage = 1;
    [SerializeField] private float attackCooldown = 2f;

    [Header("타이밍")]
    [SerializeField] private float hitDelay = 0.4f; // 애니메이션 시작 -> 판정
    [SerializeField] private float recoveryTime = 0.6f; // 판정 -> 다시 움직이기까지
    
    private IDamageable target;

    private float lastAttackTime = -999f;

    // 범위 안에 대상이 있고 쿨다운이 끝났으면 true. Controller가 추격 중에 매 프레임 확인
    public bool CanAttack
    {
        get
        {
            if (target == null) return false;
            return Time.time - lastAttackTime >= attackCooldown;
        }
    }

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


    public IEnumerator PerformAttack()
    {
        lastAttackTime = Time.time;

        yield return GameFlow.Wait(hitDelay);
        
        if (target != null && !state.IsDead)
            target.TakeDamage(damage, state.MyColor);

        yield return GameFlow.Wait(recoveryTime);
    }
}
