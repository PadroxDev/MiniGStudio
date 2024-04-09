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

        #endregion

        #region Chase Variables

        [SerializeField] private GolemChaseState.Descriptor _chaseDescriptor;

        #endregion

        #region Rock Throw Variables

        [SerializeField] private GolemRockThrowState.Descriptor _rockThrowDescriptor;

        #endregion

        #region Rock Howl Variables

        [SerializeField] private GolemRockHowlDesc _rockHowlDescriptor;

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

        //private void FixedUpdate()
        //{
        //    StateMachine.CurrentEnemyState.PhysicsUpdate();
        //}

        #region Animation Triggers

        private void AnimationTriggerEvent(AnimationTriggerType triggerType)
        {
            StateMachine.CurrentEnemyState.AnimationTriggerEvent(triggerType);
        }

        public enum AnimationTriggerType
        {
            BirthEnded,
            RightFistEnded,
            LeftFistEnded
            RockHowlBegin,
            RockHowlEnd
        }

        #endregion

        #region Movement

        public void MoveEnemy(Vector3 velocity)
        {
            RB.velocity = velocity;
        }

        #endregion
    }
}
