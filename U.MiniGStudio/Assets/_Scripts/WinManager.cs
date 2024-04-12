using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MiniGStudio
{
    public class WinManager : MonoBehaviour {
        [SerializeField] private float _pillarCount = 3f;
        [SerializeField] private Golem _golem;

        [SerializeField] private EnvironementManager _environmentManager;
        [SerializeField] private AudioClip _lostClip;
        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private GameObject _creditsCanvas;

        public void DestroyPillar() {
            _pillarCount--;
            if (_pillarCount <= 0) StartCoroutine(Win());
        }

        private IEnumerator Win() {
            _golem.enabled = false;
            _golem.Animator.SetTrigger("Death");
            yield return Helpers.GetWait(3);
            _environmentManager.gameObject.SetActive(false);
            _musicSource.Stop();
            _musicSource.clip = _lostClip;
            _musicSource.Play();
            _creditsCanvas.SetActive(true);

            yield return Helpers.GetWait(25);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
