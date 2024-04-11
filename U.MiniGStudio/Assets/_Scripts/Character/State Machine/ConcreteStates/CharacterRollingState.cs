using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiniGStudio
{
    public class CharacterRollingState : CharacterState
    {
        const string ROLLING_TRIGGER = "Roll";

        [System.Serializable]
        public struct Descriptor
        {
            public float RollPower;
            public Transform CameraTransform;
            public float Speed;
            public float MaxSpeed;
            public AudioClip rollSound;
        }

        private Descriptor _desc;

        public CharacterRollingState(Character character, CharacterStateMachine characterStateMachine, Descriptor desc) : base(character, characterStateMachine)
        {
            _desc = desc;
        }

        public override void AnimationTriggerEvent(Character.AnimationTriggerType triggerType)
        {
            base.AnimationTriggerEvent(triggerType);
            if (triggerType != Character.AnimationTriggerType.FromRollToMoving) return;
            characterStateMachine.ChangeState(character.MoveState);
        }

        public override void EnterState()
        {
            base.EnterState();
            character.Animator.SetTrigger(ROLLING_TRIGGER);
            character.RB.AddForce(_desc.CameraTransform.forward * _desc.RollPower, ForceMode.Impulse);
            character.IsDamageable = false;
        }

        public override void ExitState()
        {
            base.ExitState();
            character.Animator.ResetTrigger(ROLLING_TRIGGER);
            character.IsDamageable = true;
        }

        public override void FrameUpdate()
        {
            base.FrameUpdate();

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
