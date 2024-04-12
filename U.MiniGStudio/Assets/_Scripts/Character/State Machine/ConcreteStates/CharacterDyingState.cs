using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MiniGStudio
{
    public class CharacterDyingState : CharacterState
    {
        const string DYING_TRIGGER = "Die";
        public CharacterDyingState(Character character, CharacterStateMachine characterStateMachine) : base(character, characterStateMachine)
        {
        }

        public override void AnimationTriggerEvent(Character.AnimationTriggerType triggerType)
        {
            base.AnimationTriggerEvent(triggerType);
        }

        public override void EnterState()
        {
            base.EnterState();
            character.Animator.SetTrigger(DYING_TRIGGER);
            character.StartCoroutine(RefreshGame());
        }

        public override void ExitState()
        {
            base.ExitState();
        }

        public override void FrameUpdate()
        {
            base.FrameUpdate();
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }

        private IEnumerator RefreshGame() {
            character.GolemComponent.enabled = false;
            yield return Helpers.GetWait(3f);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
