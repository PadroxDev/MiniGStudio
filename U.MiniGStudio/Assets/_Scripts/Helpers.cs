using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiniGStudio
{
    public static class Helpers
    {
        public static Vector3 RandomPointInCircle(Vector3 center, float radius)
        {
            float angle = Random.value * Mathf.PI * 2;
            float x = center.x + Mathf.Sin(angle) * radius;
            float z = center.z + Mathf.Cos(angle) * radius;
            return new Vector3(x, center.y, z);
        }
    }
}
