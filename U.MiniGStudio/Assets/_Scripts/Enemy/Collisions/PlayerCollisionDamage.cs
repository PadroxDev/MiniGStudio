using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiniGStudio
{
    public class PlayerCollisionDamage : MonoBehaviour
    {
        private void OnTriggerEnter(Collider collision)
        {
            if (!collision.TryGetComponent(out Character character)) return;
            character.Damage(1);
        }
    }
}
