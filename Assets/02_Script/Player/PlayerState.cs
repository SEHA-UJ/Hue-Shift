using System;
using UnityEngine;

public class PlayerState : MonoBehaviour, IDamageable
{
    [Header("참조")]
    [SerializeField] private PlayerInputHandler inputHandler;
    [SerializeField] private ColorAura aura;
    
    [Header("상태")]
    [SerializeField] private int maxHp = 5;
    [SerializeField] private ColorType startColor = ColorType.Red;

    private int currentHp;
    private ColorType currentColor;
    private bool isDead;

    #region [ Public Properties ]
    public int MaxHp { get { return maxHp; } }
    public int Current { get { return currentHp; } }
    public ColorType CurrentColor { get { return currentColor; } }
    public bool IsDead { get { return isDead; } }
    #endregion

    public event Action<int, int> OnHpChanged;  // 현재 체력, 최대체력
    public event Action<ColorType> OnColorChanged;
    public event Action OnDamaged;
    public event Action OnDied;

    void Awake()
    {
        currentHp = maxHp;
        currentColor = startColor;
    }

    void Start()
    {
        SetColor(startColor);
        OnHpChanged?.Invoke(currentHp, maxHp);
    }

    void Update()
    {
        if (isDead) return;
        
        if (inputHandler.ConsumeSwap())
            Swap();
    }

    // ===== 색 =====
    
    public void Swap()
    {
        SetColor(ColorRule.NextColor(currentColor));
    }

    private void SetColor(ColorType color)
    {
        currentColor = color;

        if (aura != null)
            aura.Show(color);

        OnColorChanged?.Invoke(color);
    }

    // ===== 피격 =====

    public void TakeDamage(int amount, ColorType attackColor)
    {
        if (isDead) return;

        if (!ColorRule.CanDamagePlayer(attackColor, currentColor)) return;

        currentHp = Mathf.Max(currentHp - amount, 0);
        OnHpChanged?.Invoke(currentHp, maxHp);
        OnDamaged?.Invoke();

        if (currentHp == 0)
            Die();
    }

    private void Die()
    {
        isDead = true;
        inputHandler.SetLocked(true);
        OnDied?.Invoke();
    }
}
