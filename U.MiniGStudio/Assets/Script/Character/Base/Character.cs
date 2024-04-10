using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace TopDownController
{
    public class Character : MonoBehaviour, IDamageable
    {
        [field: SerializeField] public float MaxHealth { get; set; } = 100f;
        public float CurrentHealth { get; set; }
        public bool IsDamageable { get; set; } = true;

        public Animator Animator;
        public PlayerController Controller;
        public Rigidbody RB;

        [SerializeField] private CharacterJumpingState.Descriptor _jumpStateDescriptor;
        [SerializeField] private CharacterMovingState.Descriptor _movingStateDescriptor;
        [SerializeField] private CharacterInAirState.Descriptor _InAirStateDescriptor;
        [SerializeField] private CharacterRollingState.Descriptor _RollingStateDescriptor;

        #region State Machine Variables

        public CharacterStateMachine StateMachine { get; set; }
        public CharacterIdleState IdleState { get; set; }
        public CharacterMovingState MoveState { get; set; }
        public CharacterDyingState DyingState { get; set; }
        public CharacterJumpingState JumpingState { get; set; }
        public CharacterInAirState InAirState { get; set; }
        public CharacterRollingState RollingState { get; set; }

        #endregion

        private void Awake()
        {
            StateMachine = new CharacterStateMachine();
            IdleState = new CharacterIdleState(this, StateMachine);
            DyingState = new CharacterDyingState(this, StateMachine);
            MoveState = new CharacterMovingState(this, StateMachine, _movingStateDescriptor);
            JumpingState = new CharacterJumpingState(this, StateMachine, _jumpStateDescriptor);
            InAirState = new CharacterInAirState(this,StateMachine, _InAirStateDescriptor);
            RollingState = new CharacterRollingState(this, StateMachine, _RollingStateDescriptor);
        }

        private void Start()
        {
            CurrentHealth = MaxHealth;
            StateMachine.Initialize(IdleState);
        }

        private void Update()
        {
            StateMachine.CurrentCharacterState.FrameUpdate();
            if (Input.GetKey("up"))
            {
                StateMachine.ChangeState(DyingState);
            }

        }

        private void FixedUpdate()
        {
            StateMachine.CurrentCharacterState.PhysicsUpdate();
        }

        public void Damage(float amount)
        {
            if (!IsDamageable) return;
            CurrentHealth -= amount;

            if (CurrentHealth < 0f)
            {
                Die();
            }
        }

        public void Die()
        {
            StateMachine.ChangeState(DyingState);
        }

        private void AnimationTriggerEvent(AnimationTriggerType triggerType)
        {
            StateMachine.CurrentCharacterState.AnimationTriggerEvent(triggerType);
        }
        public void ChangeCharacterState(CharacterState state)
        {
            StateMachine.ChangeState(state);
        }

        public enum AnimationTriggerType
        {
            FromJumpToInAir,
            FromRollToMoving
        }
    }
}
