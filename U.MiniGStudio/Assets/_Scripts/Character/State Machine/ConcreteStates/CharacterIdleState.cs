using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiniGStudio
{
    public class CharacterIdleState : CharacterState
    {
        const string SPEED_PARAM = "Speed";

        Vector2 _moveDir;

        public CharacterIdleState(Character character, CharacterStateMachine characterStateMachine) : base(character, characterStateMachine)
        {
        }

        public override void AnimationTriggerEvent(Character.AnimationTriggerType triggerType)
        {
            base.AnimationTriggerEvent(triggerType);
        }

        public override void EnterState()
        {
            base.EnterState();
            Vector2 moveDir = character.Controller.GetMoveDirection();

            character.Animator.SetFloat(SPEED_PARAM, moveDir.magnitude);
            character.Controller.onJump += OnJump;
        }

        public override void ExitState()
        {
            base.ExitState();
            character.Controller.onJump -= OnJump;
        }

        public override void FrameUpdate()
        {
            base.FrameUpdate();

            _moveDir = character.Controller.GetMoveDirection();
            if (_moveDir == Vector2.zero) return;

            characterStateMachine.ChangeState(character.MoveState);
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }

        private void OnJump()
        {
            character.StateMachine.ChangeState(character.JumpingState);
        }
    }
}
