using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiniGStudio
{
    public class GolemGroundSmashState : EnemyState
    {
        [System.Serializable]
        public struct Descriptor
        {
        }

        private const string GROUND_SMASH_ANIM_PARAM = "GroundSmash";

        private int _smashNumber;

        private Descriptor _desc;

        public GolemGroundSmashState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
        {
        }

        public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
        {
            base.AnimationTriggerEvent(triggerType);
            /*
            switch (triggerType)
            {
                case Enemy.AnimationTriggerType.GroundSmashEnded:
                    _enemyStateMachine.ChangeState(_enemy.ChaseState);
                    break;
                case Enemy.AnimationTriggerType.LeftFistEnded:
                    _enemyStateMachine.ChangeState(_enemy.ChaseState);
                    break;
                default:
                    break;
            }
            */
            //_enemyStateMachine.ChangeState(_enemy.ChaseState);
        }

        public override void EnterState()
        {
            base.EnterState();

            _smashNumber = Random.Range(0, 3);
        }

        public override void ExitState()
        {
            base.ExitState();
        }

        public override void FrameUpdate()
        {
            base.FrameUpdate();
            _enemy.Animator.SetTrigger(GROUND_SMASH_ANIM_PARAM);
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }
    }
}
