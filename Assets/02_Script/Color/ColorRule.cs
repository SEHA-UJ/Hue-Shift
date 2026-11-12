using UnityEngine;

public static class ColorRule
{
    // 플레이어 공격 → 적/보스 : 같은 색이어 데미지
    public static bool CanDamageEnemy(ColorType attackColor, ColorType enemyColor)
    {
        return attackColor == enemyColor;
    }

    // 적 공격 → 플레이어 : 다른 색이어야 데미지
    public static bool CanDamagePlayer(ColorType attackColor, ColorType playerColor)
    {
        return attackColor != playerColor;
    }

    // 컬러 스왑 순서
    public static ColorType NextColor(ColorType current)
    {
        switch (current)
        {
            case ColorType.Red: return ColorType.Blue;
            case ColorType.Blue: return ColorType.Green;
            case ColorType.Green: return ColorType.Red;
            default: return ColorType.Red;
        }
    }

    // 현재 색상을 제외한 색 중 하나를 랜덤하게 결과값을 냄
    public static ColorType PickOtherColor(ColorType exclude)
    {
        ColorType first = NextColor(exclude);
        ColorType second = NextColor(first);

        if (Random.value < 0.5)
            return first;

        return second;
    }
}
