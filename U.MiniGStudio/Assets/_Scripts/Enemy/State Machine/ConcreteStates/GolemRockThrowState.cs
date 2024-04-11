using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace MiniGStudio
{
    public class GolemRockThrowState : EnemyState
    {
        [System.Serializable]
        public struct Descriptor
        {
            public float ThrowStrength;
            public float MoveSpeed;
            public float GrabDistance;
            public Transform GolemHand;
            public VisualEffect DustVFX;
        }

        private enum State
        {
            Trace,
            Grabbing,
            Throwing,
            Done
        }

        public Rock CurrentThrowableRock;

        private Descriptor _desc;
        private State _state;

        private int _grabRockHash;
        private int _speedHash;

        public GolemRockThrowState(Enemy enemy, EnemyStateMachine enemyStateMachine, Descriptor desc) : base(enemy, enemyStateMachine)
        {
            _desc = desc;
            CurrentThrowableRock = null;

            _grabRockHash = Animator.StringToHash("GrabRock");
            _speedHash = Animator.StringToHash("Speed");
        }

        public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
        {
            base.AnimationTriggerEvent(triggerType);
            switch(triggerType) {
                case Enemy.AnimationTriggerType.GrabRock:
                    GrabRock();
                    break;
                case Enemy.AnimationTriggerType.ThrowRock:
                    ThrowRock();
                    break;
                case Enemy.AnimationTriggerType.ThrowEnded:
                    ChangeToChaseState();
                    break;
                case Enemy.AnimationTriggerType.GrabDust:
                    SpawnDustVFX();
                    break;
                default:
                    break;
            }
        }

        public override void EnterState()
        {
            base.EnterState();
            _state = State.Trace;

            if (CurrentThrowableRock == null)
            {
                ChangeToChaseState();
            }
        }

        public override void ExitState()
        {
            base.ExitState();
        }

        public override void FrameUpdate()
        {
            base.FrameUpdate();

            switch(_state) { 
                case State.Trace:
                    MoveTowardsRock();
                    break;
                case State.Grabbing:
                    Vector3 dir = (CurrentThrowableRock.transform.position - _enemy.transform.position).normalized;
                    _enemy.RotateEnemy(dir);
                    break;
                case State.Throwing:
                    dir = (_enemy.PlayerRB.position - _enemy.transform.position).normalized;
                    _enemy.RotateEnemy(dir);
                    break;
                case State.Done:
                    break;
            }
        }

        private void MoveTowardsRock()
        {
            _enemy.Animator.SetFloat(_speedHash, 1);

            Vector3 dir = (CurrentThrowableRock.transform.position - _enemy.transform.position);
            dir.y = 0;
            dir.Normalize();

            _enemy.MoveEnemy(dir * _desc.MoveSpeed);
            _enemy.RotateEnemy(dir);

            float distance = Vector3.Distance(_enemy.transform.position, CurrentThrowableRock.transform.position);
            if(distance <= _desc.GrabDistance)
            {
                _enemy.Animator.SetTrigger(_grabRockHash);
                _state = State.Grabbing;
            }
        }

        public void GrabRock()
        {
            CurrentThrowableRock.transform.parent = _desc.GolemHand;
            CurrentThrowableRock.transform.localPosition = Vector3.zero;
            if (CurrentThrowableRock.TryGetComponent(out Rigidbody rb)) {
                rb.isKinematic = true;
            }
            if(CurrentThrowableRock.TryGetComponent(out Collider collider)) {
                collider.enabled = false;
            }
            _state = State.Throwing;
        }

        public void ThrowRock()
        {
            Vector3 dir = (_enemy.PlayerRB.position - _enemy.RB.position).normalized;
            _enemy.transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
            if (CurrentThrowableRock.TryGetComponent(out Rigidbody rb))
            {
                rb.isKinematic = false;
                CurrentThrowableRock.transform.parent = null;
                rb.velocity = Vector3.one * 0.01f;
                rb.AddForce(dir * _desc.ThrowStrength, ForceMode.Impulse);
            }
            if (CurrentThrowableRock.TryGetComponent(out Collider collider)) {
                collider.enabled = true;
            }
            CurrentThrowableRock.EnableDebris();
            CurrentThrowableRock.Thrown = true;
            _state = State.Done;
        }

        public void ChangeToChaseState()
        {
            _enemyStateMachine.ChangeState(_enemy.ChaseState);
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }

        public void SpawnDustVFX()
        {
            Vector3 pos = CurrentThrowableRock.transform.position - CurrentThrowableRock.transform.localScale * 0.4f;
            VisualEffect dust = GameObject.Instantiate(_desc.DustVFX, pos, Quaternion.identity, Helpers.VFXParent);
            GameObject.Destroy(dust, 4f);
        }
    }
}
