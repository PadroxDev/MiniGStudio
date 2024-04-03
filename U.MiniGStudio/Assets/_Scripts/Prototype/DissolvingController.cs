using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

namespace MiniGStudio {
    public class DissolvingController : MonoBehaviour {
        public SkinnedMeshRenderer SkinnedMesh;
        public VisualEffect VFXGraph;
        public float DissolveRate = 0.0125f;
        public float RefreshRate = 0.025f;

        private Material[] _skinnedMaterials;

        private void Awake() {
            if (SkinnedMesh is null) return;
            _skinnedMaterials = SkinnedMesh.materials;
        }

        private void Update() {
            if(Input.GetKeyDown(KeyCode.Space)) {
                StartCoroutine(DissolveCoroutine());
            }
        }

        private IEnumerator DissolveCoroutine() {
            if(VFXGraph is not null) {
                VFXGraph.Play();
            }

            if(_skinnedMaterials.Length > 0) {
                float counter = 0;

                while (_skinnedMaterials[0].GetFloat("_DissolveAmount") < 1) {
                    counter += DissolveRate;
                    for (int i = 0; i < _skinnedMaterials.Length; i++) {
                        _skinnedMaterials[i].SetFloat("_DissolveAmount", counter);
                    }
                    yield return new WaitForSeconds(RefreshRate);
                }
            }
        }
    }
}
