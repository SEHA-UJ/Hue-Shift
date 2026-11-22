using System;
using UnityEngine;

public class EnemyState : MonoBehaviour, IDamageable
{
    [Header("체력")]
    [SerializeField] private int maxHp = 3;

    [Header("상태")]
    [SerializeField] private ColorType myColor;
    
    private int currentHp;
    private bool isDead;

    public ColorType MyColor { get { return myColor; } }
    public bool IsDead { get { return isDead; } }

    public event Action OnDamaged;
    public event Action OnDied;

    void Awake()
    {
        currentHp = maxHp;
    }
    
    public void TakeDamage(int amount, ColorType attackColor)
    {
        if (isDead) return;

        if (!ColorRule.CanDamageEnemy(attackColor, myColor)) return;

        currentHp = Mathf.Max(currentHp - amount, 0);
        OnDamaged?.Invoke();

        if (currentHp == 0)
            Die();
    }

    private void Die()
    {
        isDead = true;
        OnDied?.Invoke();
    }
}
