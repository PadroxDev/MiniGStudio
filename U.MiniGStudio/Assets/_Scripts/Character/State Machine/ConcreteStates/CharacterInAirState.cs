using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static PlayerController;

namespace MiniGStudio
{
    public class CharacterInAirState : CharacterState
    {
        const string FALLING_DIR_PARAM = "FallingDir";
        const string IS_GROUNDED_PARAM = "isGrounded";
        [System.Serializable]
        public struct Descriptor
        {
            public Transform CameraTransform;
            public float Speed;
            public float MaxSpeed;
        }

        Descriptor _desc;

        public CharacterInAirState(Character character, CharacterStateMachine characterStateMachine, Descriptor desc) : base(character, characterStateMachine)
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
        }

        public override void ExitState()
        {
            base.ExitState();
            character.Controller.onJump -= OnJump;
            character.Animator.SetBool(IS_GROUNDED_PARAM, true);
        }

        public override void FrameUpdate()
        {
            base.FrameUpdate();

            character.Animator.SetFloat(FALLING_DIR_PARAM, character.RB.velocity.y);

            bool isGrounded = CheckGrounded();
            character.Animator.SetBool(IS_GROUNDED_PARAM, isGrounded);

            if (isGrounded)
            {
                characterStateMachine.ChangeState(character.IdleState);
                return;
            }

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

        private bool CheckGrounded()
        {
            RaycastHit hit;
            float sphereRadius = 0.5f;
            if (Physics.SphereCast(character.transform.position + Vector3.up, sphereRadius, Vector3.down, out hit, Mathf.Infinity))
            {
                return true;
            }

            return false;
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
