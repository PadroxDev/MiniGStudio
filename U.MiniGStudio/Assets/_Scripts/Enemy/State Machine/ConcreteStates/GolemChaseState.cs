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

        private float InitialTime;

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
            InitialTime = _desc.SmashCooldown;
        }

        public override void ExitState()
        {
            base.ExitState();
        }

        public override void FrameUpdate()
        {
            base.FrameUpdate();

            if ((_enemy.PlayerRB.transform.position - _enemy.RB.transform.position).magnitude < _desc.FistDetectionRange)
            {
                _enemy.StateMachine.ChangeState(_enemy.RockFistState);
                return;
            }

            _desc.SmashCooldown -= Time.deltaTime;

            if (_desc.SmashCooldown <= 0.0f)
            {
                //TimerEnded();
            }

            Vector3 direction = (_enemy.PlayerRB.transform.position - _enemy.RB.transform.position).normalized;
            _enemy.transform.LookAt(_enemy.PlayerRB.transform.position, Vector3.up);
            Quaternion rot = Quaternion.LookRotation(direction, Vector3.up);
            _enemy.transform.rotation = Quaternion.Slerp(_enemy.transform.rotation, rot, 0.7f * Time.deltaTime);
            _enemy.MoveEnemy(direction * _desc.ChaseSpeed);
            _enemy.Animator.SetFloat(CHASE_ANIM_PARAM, 1);

        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }
    }
}
