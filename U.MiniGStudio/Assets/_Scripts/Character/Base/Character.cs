using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace MiniGStudio
{
    public class Character : MonoBehaviour, IDamageable
    {
        [field: SerializeField] public float MaxHealth { get; set; } = 100f;
        public float CurrentHealth { get; set; }
        public bool IsDamageable { get; set; } = true;

        public Animator Animator;
        public PlayerController Controller;
        public Rigidbody RB;

        [SerializeField] private ScreenShake _screenShake;
        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private AudioClip _OnHitClip;
        [SerializeField] private AudioClip _onDeathClip;
        [SerializeField] private AudioClip _lostClip;
        [SerializeField] private Canvas _creditsCanvas;
        [SerializeField] private EnvironementManager _environementManager;

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
        }

        private void FixedUpdate()
        {
            StateMachine.CurrentCharacterState.PhysicsUpdate();
        }

        public bool Damage(float amount)
        {
            if (!IsDamageable) return false;
            if (StateMachine.CurrentCharacterState == DyingState) return false;

            _screenShake.start = true;
            CurrentHealth -= amount;

            if (CurrentHealth < 0f)
            {
                Die();
                StartCoroutine(LostGame(5f));
            }
            else
            {
                SoundFXManager.instance.PlaySoundFXClip(_OnHitClip, transform, 1);
            }
            return true;
        }

        public void Die()
        {
            StateMachine.ChangeState(DyingState);
            SoundFXManager.instance.PlaySoundFXClip(_onDeathClip, transform, 1);
        }

        private IEnumerator LostGame(float delay)
        {
            yield return new WaitForSeconds(delay);
            _environementManager.gameObject.SetActive(false);
            _musicSource.Stop();
            _musicSource.clip = _lostClip;
            _musicSource.Play();
            _creditsCanvas.gameObject.SetActive(true);
            StartCoroutine(Quit(25f));
        }

        private IEnumerator Quit(float delay)
        {
            yield return new WaitForSeconds(delay);
            Application.Quit();
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
