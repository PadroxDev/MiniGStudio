using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiniGStudio
{
    public class GolemGroundSmashState : EnemyState
    {
        private const string GROUND_SMASH_ANIM_PARAM = "GroundSmash";

        public GolemGroundSmashState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
        {
        }

        public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
        {
            base.AnimationTriggerEvent(triggerType);
            _enemyStateMachine.ChangeState(_enemy.ChaseState);
        }

        public override void EnterState()
        {
            base.EnterState();

            int smashNumber = Random.Range(0, 3);

            if (smashNumber == 0)
            {
                _enemy.Animator.SetTrigger(GROUND_SMASH_ANIM_PARAM);
            }
            else if (smashNumber == 1)
            {

            }
            else
            {
                //_enemy.Animator.SetTrigger(LEFT_FIST_ANIM_PARAM);
            }
        }

        public override void ExitState()
        {
            base.ExitState();
        }

        public override void FrameUpdate()
        {
            base.FrameUpdate();
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }
    }
}
