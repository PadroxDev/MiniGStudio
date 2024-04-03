using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

namespace MiniGStudio {
    public class DissolvingController : MonoBehaviour {
        [SerializeField] private Renderer _renderer;
        [SerializeField] private VisualEffect _vfxGraph;
        [SerializeField] private Animator _animator;
        [SerializeField] private float _dissolveRate = 0.0125f;
        [SerializeField] private float _refreshRate = 0.025f;
        [SerializeField] private float _delay = 0.3f;

        private Material[] _skinnedMaterials;
        private float reverse = -1;

        private void Awake() {
            if (_renderer is null) return;
            _skinnedMaterials = _renderer.materials;
        }

        private void Update() {
            if(Input.GetKeyDown(KeyCode.Space)) {
                StartCoroutine(DissolveCoroutine());
            }
        }

        private IEnumerator DissolveCoroutine() {
            if (_animator)
                _animator.SetTrigger("Die");

            reverse = reverse == -1 ? 1 : -1;

            yield return new WaitForSeconds(_delay);

            if (_vfxGraph is not null)
                _vfxGraph.Play();

            if(_skinnedMaterials.Length > 0) {
                float counter = reverse == 1 ? 0 : 1;

                while (reverse == 1 ? _skinnedMaterials[0].GetFloat("_DissolveAmount") < 1 : _skinnedMaterials[0].GetFloat("_DissolveAmount") > 0) {
                    counter += _dissolveRate * reverse;
                    for (int i = 0; i < _skinnedMaterials.Length; i++) {
                        _skinnedMaterials[i].SetFloat("_DissolveAmount", counter);
                    }
                    yield return new WaitForSeconds(_refreshRate);
                }
            }
        }
    }
}
