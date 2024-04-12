using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiniGStudio
{
    public class ScreenShake : MonoBehaviour
    {
        public bool start = false;
        public AnimationCurve curve;
        public float duration = 1.0f;
        public float intensity = 1.0f;

        private void Update()
        {
            if (start)
            {
                start = false;
                StartCoroutine(Shaking());
            }
        }
        IEnumerator Shaking()
        {
            Vector3 startPosition = transform.position;
            float elapsedTime = 0.0f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float strenght = curve.Evaluate(elapsedTime / duration);
                transform.position = startPosition + Random.insideUnitSphere * (strenght * intensity);
                yield return null;
            }
            transform.position = startPosition;
        }
    }
}
