using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace MiniGStudio
{
    public class BombManager : MonoBehaviour {

        private const string BOMB_POS_TRANSFORM_NAME = "BombPos";

        [Header("Global Properties")]
        [SerializeField] private float _bombHeight;
        [SerializeField] private VisualEffect _explosionVFX;

        [Space]
        [Header("Floating Properties")]
        [SerializeField] private float _castRadius;
        [SerializeField] private LayerMask _whatIsGround;
        [SerializeField] private float _elevationForce;
        [SerializeField] private float _rotationSpeed;

        [Space]
        [Header("Follow Properties")]
        [SerializeField] private float _bombCountdown;
        [SerializeField] private float _maxFollowDelta;
        [SerializeField] private float _followSpeed = 8f;

        [Space]
        [Header("Move on Top of Pillar Properties")]
        [SerializeField] private float _moveDuration = 5f;

        [Space]
        [Header("Explode Properties")]
        [SerializeField] private float _spinDuration = 4f;
        [SerializeField] private float _minSpinSpeed = 3f;
        [SerializeField] private float _maxSpinSpeed = 9f;
        [SerializeField] private ShatteredPillar _shatteredPillar;

        private enum State {
            Floating,
            Follow,
            MoveOnTopOfPillar,
            Explode,
        }

        private State _currentState;
        private Rigidbody _rb;
        private Transform _plr;
        private Transform _pillar;
        private Vector3 _lerpStartPos;
        private Quaternion _slerpStartRot;
        private Vector3 _bombPos;
        private float _moveToPillarCountdown;
        private float _spinCountdown;
        private WinManager _winManager;

        private void Start() {
            _rb = GetComponent<Rigidbody>();


            _currentState = State.Floating;
            _moveToPillarCountdown = 0;
            _spinCountdown = 0;

            _winManager = GameObject.FindGameObjectWithTag("WinManager").GetComponent<WinManager>();
        }

        private void FixedUpdate() {
            switch (_currentState) {
                case State.Floating:
                    HandleFloating();
                    break;
                case State.Follow:
                    HandleFollow();
                    break;
                case State.MoveOnTopOfPillar:
                    HandleMoveOnTopOfPillar();
                    break;
                case State.Explode:
                    HandleExplode();
                    break;
            }
        }

        private void HandleFloating() {
            ElevateOverSphereCast();
        }

        private void HandleFollow() {
            ElevateOverSphereCast();

            Vector3 dir = (_plr.position - transform.position);
            dir.y = 0;
            float distance = dir.magnitude;
            dir.Normalize();

            float dT = Helpers.Clamp(distance, _followSpeed, _maxFollowDelta);
            Vector3 force = dir * dT;
            _rb.AddForce(force, ForceMode.Force);
        }

        private void HandleMoveOnTopOfPillar() {
            _moveToPillarCountdown += Time.fixedDeltaTime;

            float alpha = Mathf.Clamp01(_moveToPillarCountdown / _moveDuration);
            Vector3 pos = Vector3.Lerp(_lerpStartPos, _bombPos, alpha);
            _rb.position = pos;

            Quaternion rot = Quaternion.Slerp(_slerpStartRot, Quaternion.identity, alpha);
            _rb.rotation = rot;

            if (_moveToPillarCountdown < _moveDuration) return;
            _currentState = State.Explode;
        }

        private void HandleExplode() {
            _spinCountdown += Time.fixedDeltaTime;

            float alpha = Mathf.Clamp01(_spinCountdown / _spinDuration);
            float spinSpeed = Mathf.Lerp(_minSpinSpeed, _maxSpinSpeed, alpha);
            Quaternion rot = Quaternion.Euler(0, spinSpeed, 0);
            transform.rotation *= rot;

            if (_spinCountdown < _spinDuration) return;
            Explode();
        }

        private void Explode() {
            VisualEffect vfx = Instantiate(_explosionVFX, transform.position, Quaternion.identity, Helpers.VFXParent);
            Destroy(vfx, 4f);
            ShatteredPillar shatteredPillar = Instantiate(_shatteredPillar, _pillar.position, _pillar.rotation);
            shatteredPillar.Shatter(transform.position);

            _winManager.DestroyPillar();
            Destroy(_pillar.gameObject);

            Destroy(gameObject);
        }

        private void ElevateOverSphereCast() {
            RaycastHit hit;
            if (!Physics.SphereCast(transform.position, _castRadius, Vector3.down, out hit, _bombHeight, _whatIsGround, QueryTriggerInteraction.Ignore)) return;

            float dT = -Helpers.Clamp(-hit.distance, 0, -6);
            _rb.AddForce(Vector3.up * _elevationForce * dT, ForceMode.Force);
        }

        private void OnCollisionEnter(Collision collision) {
            VisualEffect vfx = Instantiate(_explosionVFX, transform.position, Quaternion.identity, Helpers.VFXParent);
            Destroy(vfx, 4f);
            Destroy(gameObject);
        }

        private void OnTriggerEnter(Collider other) {
            if (other.transform.tag == "Pillar") {
                if (_currentState != State.Follow) return;
                _pillar = other.transform;
                _lerpStartPos = transform.position;
                _slerpStartRot = transform.rotation;
                _bombPos = _pillar.Find(BOMB_POS_TRANSFORM_NAME).transform.position;
                _currentState = State.MoveOnTopOfPillar;
                _rb.velocity = Vector3.zero;
                _rb.isKinematic = true;
            }

            if (other.transform.tag == "Player")  // Change with Character Component
            {
                if (_currentState != State.Floating) return;
                _plr = other.transform;
                _currentState = State.Follow;
            }
        }

        private void OnDrawGizmosSelected() {
            //// Floating Raycast ////
            // Line //
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.position - Vector3.up * _bombHeight);

            // Sphere //
            Gizmos.DrawWireSphere(transform.position, _castRadius);
        }
    }
}
