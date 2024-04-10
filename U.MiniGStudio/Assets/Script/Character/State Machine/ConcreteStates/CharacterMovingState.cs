using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerController;

namespace TopDownController
{
    public class CharacterMovingState : CharacterState
    {
        const string SPEED_PARAM = "Speed";
        const string DIRX_PARAM = "DirX";
        const string DIRY_PARAM = "DirY";

        [System.Serializable]
        public struct Descriptor
        {
            public Transform CameraTransform;
            public float Speed;
            public float MaxSpeed;
        }

        Descriptor _desc;

        public CharacterMovingState(Character character, CharacterStateMachine characterStateMachine, Descriptor desc) : base(character, characterStateMachine)
        {
            _desc = desc;
        }

        public override void AnimationTriggerEvent(Character.AnimationTriggerType triggerType)
        {
            base.AnimationTriggerEvent(triggerType);
        }

        public override void EnterState()
        {
            base.EnterState();
            character.Controller.onJump += OnJump;
            character.Controller.onRoll += OnRoll;
        }

        public override void ExitState()
        {
            base.ExitState();
            character.Controller.onJump -= OnJump;
            character.Controller.onRoll -= OnRoll;
        }

        public override void FrameUpdate()
        {
            base.FrameUpdate();



            // FALLING
            if (character.RB.velocity.y < -0.1)
            {
                characterStateMachine.ChangeState(character.InAirState);
            }

            Vector2 moveDir = character.Controller.GetMoveDirection();

            character.Animator.SetFloat(SPEED_PARAM, moveDir.magnitude);
            character.Animator.SetFloat(DIRX_PARAM, moveDir.x);
            character.Animator.SetFloat(DIRY_PARAM, moveDir.y);

            if (moveDir == Vector2.zero)
            {
                characterStateMachine.ChangeState(character.IdleState);
                return;
            }

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
        private void OnJump()
        {
            character.StateMachine.ChangeState(character.JumpingState);
        }

        private void OnRoll()
        {
            character.StateMachine.ChangeState(character.RollingState);
        }
    }
}
