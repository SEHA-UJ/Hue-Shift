using UnityEngine;

public class Scarecrow : MonoBehaviour, IDamageable
{
    [SerializeField] private ColorType color = ColorType.Red;

    private int hitCount;

    public void TakeDamage(int amount, ColorType attackColor)
    {
        if (!ColorRule.CanDamageEnemy(attackColor, color))
        {
            Debug.Log($"[허수아비 {color}] {attackColor} 공격 → 무효", this);
            return;
        }

        hitCount++;
        Debug.Log($"[허수아비 {color}] {attackColor} 공격 → 적중! 데미지 {amount} (누적 {hitCount}회)", this);
    }
}
