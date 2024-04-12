using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiniGStudio
{
    public class PlayerCollisionDamage : MonoBehaviour
    {
        private void OnTriggerEnter(Collider collision)
        {
            Character character = collision.GetComponentInChildren<Character>();
            if (!character) return;
            character.Damage(1.0f);
        }
    }
}
