using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiniGStudio
{
    public class GolemChaseState : EnemyState
    {
        [System.Serializable]
        public struct Descriptor
        {
            public float ChaseSpeed;
            public float FistDetectionRange;
            public float SmashCooldown;
            public float MinimumSmashDistance;
        }

        private const string CHASE_ANIM_PARAM = "Speed";

        private float _elapsedTime;

        private Descriptor _desc;

        public GolemChaseState(Enemy enemy, EnemyStateMachine enemyStateMachine, Descriptor desc) : base(enemy, enemyStateMachine)
        {
            _desc = desc;
        }

        public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
        {
            base.AnimationTriggerEvent(triggerType);
        }

        public override void EnterState()
        {
            base.EnterState();
            _enemy.Animator.SetFloat(CHASE_ANIM_PARAM, 0);
            _elapsedTime = 0.0f;
        }

        public override void ExitState()
        {
            base.ExitState();
        }

        public override void FrameUpdate()
        {
            base.FrameUpdate();

            float distance = Vector3.Distance(_enemy.PlayerRB.transform.position, _enemy.RB.transform.position);
            if (distance < _desc.FistDetectionRange)
            {
                _enemyStateMachine.ChangeState(_enemy.RockFistState);
                return;
            }

            _elapsedTime += Time.deltaTime;

            if (_desc.SmashCooldown <= _elapsedTime)
            {

                if (distance >= _desc.MinimumSmashDistance)
                {
                    _elapsedTime = 0.0f;
                    _enemyStateMachine.ChangeState(_enemy.GroundSmashState);
                }
            }

            Vector3 direction = (_enemy.PlayerRB.transform.position - _enemy.RB.transform.position).normalized;
            _enemy.transform.LookAt(_enemy.PlayerRB.transform.position, Vector3.up);
            _enemy.MoveEnemy(direction * _desc.ChaseSpeed);
            _enemy.RotateEnemy(direction);
            _enemy.Animator.SetFloat(CHASE_ANIM_PARAM, 1);

        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }
    }
}
