using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiniGStudio
{
    public class ShatteredPillar : MonoBehaviour {
        [SerializeField] private float _minExplosionForce = 3f;
        [SerializeField] private float _maxExplosionForce = 10f;
        [SerializeField] private float _explosionRadius = 5f;

        public void Shatter(Vector3 pos) {
            foreach(Transform t in transform) {
                var rb = t.GetComponent<Rigidbody>();

                if (rb == null) continue;
                rb.AddExplosionForce(Random.Range(_minExplosionForce, _maxExplosionForce), pos, _explosionRadius);
            }
        }
    }
}
