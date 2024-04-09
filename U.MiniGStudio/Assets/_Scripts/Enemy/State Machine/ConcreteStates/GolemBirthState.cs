using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiniGStudio
{
    public class GolemBirthState : EnemyState
    {
        private const string RISE_ANIM_PARAM = "Rise";

        public GolemBirthState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
        {
        }

        public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
        {
            base.AnimationTriggerEvent(triggerType);
            if (triggerType != Enemy.AnimationTriggerType.BirthEnded) return;
            _enemyStateMachine.ChangeState(_enemy.ChaseState);
        }

        public override void EnterState()
        {
            base.EnterState();
            _enemy.Animator.SetTrigger(RISE_ANIM_PARAM);
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
