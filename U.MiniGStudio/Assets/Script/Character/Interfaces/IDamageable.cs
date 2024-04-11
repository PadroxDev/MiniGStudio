using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiniGStudio
{
    public interface IDamageable
    {
        void Damage(float amount);

        void Die();
        bool IsDamageable { get; set; }

        float MaxHealth { get; set; }
        float CurrentHealth { get; set; }     
    }
}
