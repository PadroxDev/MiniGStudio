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
        }

        public Rock CurrentThrowableRock;

        private Descriptor _desc;
        private bool _grabbedRock;

        public GolemRockThrowState(Enemy enemy, EnemyStateMachine enemyStateMachine, Descriptor desc) : base(enemy, enemyStateMachine)
        {
            _desc = desc;
            CurrentThrowableRock = null;
        }

        public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
        {
            base.AnimationTriggerEvent(triggerType);
        }

        public override void EnterState()
        {
            base.EnterState();
            _grabbedRock = false;

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

            if(!_grabbedRock)
            {
                MoveTowardsRock();
            } else
            {

            }
        }

        private void MoveTowardsRock()
        {
            Vector3 dir = (CurrentThrowableRock.transform.position - _enemy.transform.position);
            dir.y = 0;
            dir.Normalize();

            _enemy.MoveEnemy(dir * _desc.MoveSpeed);
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
