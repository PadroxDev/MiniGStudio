using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiniGStudio
{
    public class Enemy : MonoBehaviour, IEnemyMovable
    {
        public Rigidbody RB { get; set; }

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

        #region Idle Variables

        #endregion

        #region Rock Howl Variables

        [SerializeField] private GolemRockHowlDesc RockHowlDescriptor;

        #endregion

        private void Awake()
        {
            StateMachine = new EnemyStateMachine();

            BirthState = new GolemBirthState(this, StateMachine);
            ChaseState = new GolemChaseState(this, StateMachine);
            DeathState = new GolemDeathState(this, StateMachine);
            IdleState = new GolemIdleState(this, StateMachine);
            RockFistState = new GolemRockFistState(this, StateMachine);
            RockThrowState = new GolemRockThrowState(this, StateMachine);
            RockHowlState = new GolemRockHowlState(this, StateMachine, RockHowlDescriptor);
        }

        private void Start()
        {
            RB = GetComponent<Rigidbody>();

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
            EnemyDamaged,
            PlayFootstepSound
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
