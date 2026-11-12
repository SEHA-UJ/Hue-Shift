using System;
using UnityEngine;

[Serializable]
public class ColorSet<T>
{
    [SerializeField] private T red;
    [SerializeField] private T blue;
    [SerializeField] private T green;

    public T Get(ColorType color)
    {
        switch (color)
        {
            case ColorType.Red: return red;
            case ColorType.Blue: return blue;
            case ColorType.Green: return green;
            default: return default(T);
        }
    }
}
