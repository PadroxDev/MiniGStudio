using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiniGStudio
{
    public class TempPlayerDeplacement : MonoBehaviour
    {
        public float speed;
        public Rigidbody body;

        private Vector3 direction;

        void Start()
        {
            direction = Vector3.zero;
        }

        void Update()
        {
            direction = Vector3.zero;

            if (!Input.anyKey) return;

            if (Input.GetKey(KeyCode.S))
                direction.z -= 1;
            if (Input.GetKey(KeyCode.D)) 
                direction.x += 1;
            if (Input.GetKey(KeyCode.W))
                direction.z += 1;
            if (Input.GetKey(KeyCode.A))
                direction.x -= 1;

            body.velocity = direction.normalized * speed;
        }
    }
}
