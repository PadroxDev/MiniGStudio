using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiniGStudio
{
    public static class Helpers
    {
        public static Dictionary<float, WaitForSeconds> WaitDictionary = new();
        public static WaitForSeconds GetWait(float time)
        {
            if(WaitDictionary.TryGetValue(time, out WaitForSeconds wait))
                return wait;

            WaitDictionary[time] = new WaitForSeconds(time);
            return WaitDictionary[time];
        }
        
        private static Transform _vfxParent;
        public static Transform VFXParent
        {
            get
            {
                if (_vfxParent == null) _vfxParent = new GameObject("VFX").transform;
                return _vfxParent;
            }
        }

        public static Vector3 RandomPointInCircle(Vector3 center, float radius)
        {
            float angle = Random.value * Mathf.PI * 2;
            float x = center.x + Mathf.Sin(angle) * radius;
            float z = center.z + Mathf.Cos(angle) * radius;
            return new Vector3(x, center.y, z);
        }
    }
}
