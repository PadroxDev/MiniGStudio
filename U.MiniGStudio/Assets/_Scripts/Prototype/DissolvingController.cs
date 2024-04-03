using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

namespace MiniGStudio {
    public class DissolvingController : MonoBehaviour {
        [SerializeField] private SkinnedMeshRenderer _skinnedMesh;
        [SerializeField] private VisualEffect _vfxGraph;
        [SerializeField] private Animator _animator;
        [SerializeField] private float _dissolveRate = 0.0125f;
        [SerializeField] private float _refreshRate = 0.025f;
        [SerializeField] private float _delay = 0.3f;

        private Material[] _skinnedMaterials;

        private void Awake() {
            if (_skinnedMesh is null) return;
            _skinnedMaterials = _skinnedMesh.materials;
        }

        private void Update() {
            if(Input.GetKeyDown(KeyCode.Space)) {
                StartCoroutine(DissolveCoroutine());
            }
        }

        private IEnumerator DissolveCoroutine() {
            if (_animator is not null)
                _animator.SetTrigger("Die");

            yield return new WaitForSeconds(_delay);

            if (_vfxGraph is not null)
                _vfxGraph.Play();

            if(_skinnedMaterials.Length > 0) {
                float counter = 0;

                while (_skinnedMaterials[0].GetFloat("_DissolveAmount") < 1) {
                    counter += _dissolveRate;
                    for (int i = 0; i < _skinnedMaterials.Length; i++) {
                        _skinnedMaterials[i].SetFloat("_DissolveAmount", counter);
                    }
                    yield return new WaitForSeconds(_refreshRate);
                }
            }
        }
    }
}
