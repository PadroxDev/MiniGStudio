using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiniGStudio
{
    public class GolemRockFistState : EnemyState
    {
        private const string RIGHT_FIST_ANIM_PARAM = "RightFist";
        private const string LEFT_FIST_ANIM_PARAM = "LeftFist";

        public GolemRockFistState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
        {
        }

        public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
        {
            base.AnimationTriggerEvent(triggerType);
            switch(triggerType)
            {
                case Enemy.AnimationTriggerType.RightFistEnded:
                    _enemyStateMachine.ChangeState(_enemy.ChaseState);
                    break;
                case Enemy.AnimationTriggerType.LeftFistEnded:
                    _enemyStateMachine.ChangeState(_enemy.ChaseState);
                    break;
                default:
                    break;
            }
        }

        public override void EnterState()
        {
            base.EnterState();

            int isRight = Random.Range(0, 2);

            if (isRight == 0)
            {
                _enemy.Animator.SetTrigger(RIGHT_FIST_ANIM_PARAM);
            }
            else
            {
                _enemy.Animator.SetTrigger(LEFT_FIST_ANIM_PARAM);
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
