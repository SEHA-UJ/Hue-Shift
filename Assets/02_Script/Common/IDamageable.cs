using System;
using UnityEngine;

public interface IDamageable
{
   event Action OnDamaged;
   void TakeDamage(int amount, ColorType attackColor);
}
