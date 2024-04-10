using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TopDownController
{
    public class CharacterJumpingState : CharacterState
    {
        const string FALLING_DIR_PARAM = "FallingDir";
        const string IS_GROUNDED_PARAM = "isGrounded";

        [System.Serializable]
        public struct Descriptor
        {
            public float JumpPower;
            public Transform CameraTransform;
            public float Speed;
            public float MaxSpeed;
        }

        private Descriptor _desc;

        public CharacterJumpingState(Character character, CharacterStateMachine characterStateMachine, Descriptor desc) : base(character, characterStateMachine)
        {
            _desc = desc;
        }

        public override void AnimationTriggerEvent(Character.AnimationTriggerType triggerType)
        {
            base.AnimationTriggerEvent(triggerType);
            if (triggerType != Character.AnimationTriggerType.FromJumpToInAir) return;
            characterStateMachine.ChangeState(character.InAirState);
        }

        public override void EnterState()
        {
            base.EnterState();
            character.Animator.SetBool(IS_GROUNDED_PARAM, false);
            character.RB.AddForce(Vector3.up * _desc.JumpPower, ForceMode.Impulse);
        }

        public override void ExitState()
        {
            base.ExitState();
        }

        public override void FrameUpdate()
        {
            base.FrameUpdate();
            character.Animator.SetFloat(FALLING_DIR_PARAM, character.RB.velocity.y);

            Vector2 moveDir = character.Controller.GetMoveDirection();

            Vector3 cameraForward = _desc.CameraTransform.forward;
            cameraForward.y = 0f;
            cameraForward.Normalize();

            Vector3 moveDirRelative = cameraForward * moveDir.y + _desc.CameraTransform.right * moveDir.x;
            moveDirRelative.Normalize();

            character.RB.AddForce(moveDirRelative * _desc.Speed, ForceMode.Force);

            Vector3 currentVel = character.RB.velocity;
            float yVelocity = character.RB.velocity.y;
            currentVel.y = 0;

            float currentSpeed = currentVel.magnitude;
            if (currentSpeed > _desc.MaxSpeed)
            {
                currentVel = currentVel.normalized * _desc.MaxSpeed;
            }

            if (currentVel != Vector3.zero)
            {
                character.transform.rotation = Quaternion.LookRotation(currentVel);
            }
            currentVel.y = yVelocity;
            character.RB.velocity = currentVel;
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }
    }
}
