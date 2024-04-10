using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiniGStudio
{
    public class GolemGroundSmashState : EnemyState
    {
        [System.Serializable]
        public struct Descriptor
        {
            public Spike[] SpikesPrefab;
            public float riseDelay;
            public float waitDelay;
            public float hideDelay;
            public float targetHeight;
        }

        private const string GROUND_SMASH_ANIM_PARAM = "GroundSmash";

        private int _smashNumber;

        private Descriptor _desc;

        public GolemGroundSmashState(Enemy enemy, EnemyStateMachine enemyStateMachine, Descriptor desc) : base(enemy, enemyStateMachine)
        {
            _desc = desc;
        }

        public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
        {
            base.AnimationTriggerEvent(triggerType);
            switch (triggerType)
            {
                case Enemy.AnimationTriggerType.GroundSmashed:
                    Vector3 playerPos = _enemy.PlayerRB.transform.position;
                    playerPos.y -= 2;
                    GameObject.Instantiate(_desc.SpikesPrefab[Random.Range(0, _desc.SpikesPrefab.Length)], playerPos, Quaternion.identity);
                    break;

                case Enemy.AnimationTriggerType.GroundSmashEnded:
                    if (_smashNumber == 0)
                        _enemyStateMachine.ChangeState(_enemy.ChaseState);
                    else _smashNumber--;
                    break;

                default:
                    break;
            }
            //_enemyStateMachine.ChangeState(_enemy.ChaseState);
        }

        public override void EnterState()
        {
            base.EnterState();
            _smashNumber = Random.Range(0, 3);
        }

        public override void ExitState()
        {
            base.ExitState();
        }

        public override void FrameUpdate()
        {
            base.FrameUpdate();
            _enemy.Animator.SetTrigger(GROUND_SMASH_ANIM_PARAM);
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }
    }
}
