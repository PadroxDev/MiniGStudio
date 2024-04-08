using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiniGStudio
{
    public class GolemChaseState : EnemyState
    {
        private float GolemChaseSpeed;

        public GolemChaseState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
        {
            GolemChaseSpeed = _enemy.speed;
        }

        public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
        {
            base.AnimationTriggerEvent(triggerType);
        }

        public override void EnterState()
        {
            base.EnterState();
            _enemy.Animator.SetFloat("Speed",1.0f);
        }

        public override void ExitState()
        {
            base.ExitState();
        }

        public override void FrameUpdate()
        {
            base.FrameUpdate();

            Vector3 direction = (_enemy.PlayerRB.transform.position - _enemy.RB.transform.position).normalized;
            _enemy.transform.LookAt(_enemy.PlayerRB.transform.position, Vector3.up);
            Quaternion rot = Quaternion.LookRotation(direction, Vector3.up);
            _enemy.transform.rotation = Quaternion.Slerp(_enemy.transform.rotation, rot, 0.7f * Time.deltaTime);
            _enemy.MoveEnemy(direction * GolemChaseSpeed);

            if ((_enemy.PlayerRB.transform.position - _enemy.RB.transform.position).magnitude < 3)
            {
                _enemy.StateMachine.ChangeState(_enemy.RockFistState);
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }
    }
}
