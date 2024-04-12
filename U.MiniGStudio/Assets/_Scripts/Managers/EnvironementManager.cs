using UnityEngine;

namespace MiniGStudio
{
    [System.Serializable]
    public struct EnvironnementDesc
    {
        public AudioSource[] audioSources;
        public AudioClip[] thunderNearSounds;
        public AudioClip[] thunderFarSounds;
    }

    public class EnvironementManager : MonoBehaviour
    {
        [SerializeField]
        private EnvironnementDesc _desc;

        private float minInterval = 5f;
        private float maxInterval = 10f;

        private void Awake()
        {
            foreach (var source in _desc.audioSources)
            {
                source.gameObject.SetActive(true);
            }
        }

        private void Start()
        {
            InvokeRepeating("PlayRandomThunderNearSound", Random.Range(minInterval, maxInterval), Random.Range(minInterval, maxInterval));
        }

        private void PlayRandomThunderNearSound()
        {
            if (_desc.thunderNearSounds.Length == 0)
            {
                return;
            }

            AudioClip randomClip = _desc.thunderNearSounds[Random.Range(0, _desc.thunderNearSounds.Length)];

            if (randomClip != null)
            {
                foreach (var source in _desc.audioSources)
                {
                    SoundFXManager.instance.PlaySoundFXClip(randomClip, source.transform, 1.0f);
                }
            }
        }

        private void OnDisable()
        {
            foreach (var source in _desc.audioSources)
            {
                source.gameObject.SetActive(false);
            }
        }
    }
}
