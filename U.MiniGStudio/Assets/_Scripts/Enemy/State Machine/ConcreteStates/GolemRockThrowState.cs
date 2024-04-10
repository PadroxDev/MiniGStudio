using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        }

        private enum State
        {
            Trace,
            Grabbing
        }

        public Rock CurrentThrowableRock;

        private Descriptor _desc;
        private int _grabRockHash;
        private State _state;

        public GolemRockThrowState(Enemy enemy, EnemyStateMachine enemyStateMachine, Descriptor desc) : base(enemy, enemyStateMachine)
        {
            _desc = desc;
            CurrentThrowableRock = null;

            _grabRockHash = Animator.StringToHash("GrabRock");
        }

        public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
        {
            base.AnimationTriggerEvent(triggerType);
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
                    Vector3 dir = (CurrentThrowableRock.transform.position - _enemy.transform.position);
                    _enemy.RotateEnemy(dir);
                    break;
            }
        }

        private void MoveTowardsRock()
        {
            Vector3 dir = (CurrentThrowableRock.transform.position - _enemy.transform.position);
            dir.y = 0;
            dir.Normalize();

            _enemy.MoveEnemy(dir * _desc.MoveSpeed);
            _enemy.RotateEnemy(dir);

            float distance = Vector3.Distance(_enemy.transform.position, _enemy.PlayerRB.position);
            if(distance <= _desc.GrabDistance)
            {
                _enemy.Animator.SetTrigger(_grabRockHash);
                _state = State.Grabbing;
            }
        }

        public void GrabRock()
        {
            CurrentThrowableRock.transform.parent = _desc.GolemHand;
        }

        public void ThrowRock()
        {
            if(CurrentThrowableRock.TryGetComponent(out Rigidbody rb))
            {
                CurrentThrowableRock.transform.parent = null;
                Vector3 dir = (_enemy.PlayerRB.position - _enemy.RB.position).normalized;
                rb.AddForce(dir * _desc.ThrowStrength, ForceMode.Impulse);
                CurrentThrowableRock.Thrown = true;
            }
        }

        public void ChangeToChaseState()
        {
            _enemyStateMachine.ChangeState(_enemy.ChaseState);
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }
    }
}
