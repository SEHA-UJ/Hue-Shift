using UnityEngine;

public class ColorAura : MonoBehaviour
{
    [SerializeField] private ColorSet<GameObject> auras;

    public void Show(ColorType color)
    {
        Hide();
        SetAuraActive(color, true);
    }

    public void Hide()
    {
        SetAuraActive(ColorType.Red, false);
        SetAuraActive(ColorType.Blue, false);
        SetAuraActive(ColorType.Green, false);
    }

    public void SetAuraActive(ColorType color, bool isActive)
    {
        GameObject aura = auras.Get(color);

        if (aura != null)
            aura.SetActive(isActive);
    }
}
