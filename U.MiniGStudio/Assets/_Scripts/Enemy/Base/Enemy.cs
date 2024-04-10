using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiniGStudio
{
    public class Enemy : MonoBehaviour, IEnemyMovable
    {
        public Rigidbody RB { get; set; }
        public Animator Animator { get; set; }

        public Rigidbody PlayerRB;

        #region State Machine Variables

        public EnemyStateMachine StateMachine { get; set; }
        public GolemBirthState BirthState { get; set; }
        public GolemChaseState ChaseState { get; set; }
        public GolemDeathState DeathState { get; set; }
        public GolemIdleState IdleState { get; set; }
        public GolemRockFistState RockFistState { get; set; }
        public GolemRockThrowState RockThrowState { get; set; }
        public GolemRockHowlState RockHowlState { get; set; }
        public GolemGroundSmashState GroundSmashState { get; set; }

        #endregion

        #region Chase Variables

        [SerializeField] private GolemChaseState.Descriptor _chaseDescriptor;

        #endregion

        #region Ground Smash Variables

        [SerializeField] private GolemGroundSmashState.Descriptor _groundSmashDescriptor;

        #endregion

        #region Rock Throw Variables

        [SerializeField] private GolemRockThrowState.Descriptor _rockThrowDescriptor;

        #endregion

        #region Rock Howl Variables

        [SerializeField] private GolemRockHowlState.Descriptor _rockHowlDescriptor;

        #endregion

        private void Awake()
        {
            StateMachine = new EnemyStateMachine();

            BirthState = new GolemBirthState(this, StateMachine);
            ChaseState = new GolemChaseState(this, StateMachine, _chaseDescriptor);
            DeathState = new GolemDeathState(this, StateMachine);
            IdleState = new GolemIdleState(this, StateMachine);
            RockFistState = new GolemRockFistState(this, StateMachine);
            RockThrowState = new GolemRockThrowState(this, StateMachine, _rockThrowDescriptor);
            RockHowlState = new GolemRockHowlState(this, StateMachine, _rockHowlDescriptor);
            GroundSmashState = new GolemGroundSmashState(this, StateMachine, _groundSmashDescriptor);
        }

        private void Start()
        {
            RB = GetComponent<Rigidbody>();

            Animator = GetComponent<Animator>();

            StateMachine.Initialize(BirthState);
        }

        private void Update()
        {
            StateMachine.CurrentEnemyState.FrameUpdate();
        }

        private void FixedUpdate()
        {
            StateMachine.CurrentEnemyState.PhysicsUpdate();
        }

        #region Animation Triggers

        private void AnimationTriggerEvent(AnimationTriggerType triggerType)
        {
            StateMachine.CurrentEnemyState.AnimationTriggerEvent(triggerType);
        }

        public enum AnimationTriggerType
        {
            BirthEnded,
            RightFistEnded,
            LeftFistEnded,
            RockHowlBegin,
            RockHowlEnd,
            GroundSmashEnded,
            GroundSmashed
        }

        #endregion

        #region Movement

        public void MoveEnemy(Vector3 velocity)
        {
            RB.velocity = velocity;
        }

        public void RotateEnemy(Vector3 direction)
        {
            Quaternion rot = Quaternion.LookRotation(direction, Vector3.up);
            RB.MoveRotation(Quaternion.Slerp(transform.rotation, rot, 0.7f * Time.deltaTime));
        }

        #endregion
    }
}
