using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiniGStudio
{
    public class PlayerCollisionDamage : MonoBehaviour
    {
        private void OnTriggerEnter(Collider collision)
        {
            if (collision.transform.tag != "Player") return;
            {
                Debug.Log("Ouch");
                // TODO : LE JOUEUR PREND 1 DÉGÂT ICI
            }
        }
    }
}
