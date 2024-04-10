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
            public float initialHeight;
            public LayerMask whatIsGround;
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
                    RaycastHit hit;
                    if (Physics.Raycast(_enemy.PlayerRB.transform.position + Vector3.up, Vector3.down, out hit, 20, _desc.whatIsGround))
                    {
                        if (hit.collider.gameObject != _enemy.PlayerRB.gameObject)
                        {
                            Spike spikePrefab = _desc.SpikesPrefab[Random.Range(0, _desc.SpikesPrefab.Length)];
                            Spike spike = GameObject.Instantiate(spikePrefab, hit.point + Vector3.up * _desc.initialHeight, Quaternion.identity);
                            spike._desc = _desc;
                        }
                    }
                    break;

                case Enemy.AnimationTriggerType.GroundSmashEnded:
                    if (_smashNumber == 0)
                    {
                        _enemyStateMachine.ChangeState(_enemy.ChaseState);
                    }
                    else 
                    {
                        _smashNumber--;
                        _enemy.Animator.SetTrigger(GROUND_SMASH_ANIM_PARAM);
                    }
                    break;

                default:
                    break;
            }
        }

        public override void EnterState()
        {
            base.EnterState();
            _smashNumber = Random.Range(0, 3);
            _enemy.Animator.SetTrigger(GROUND_SMASH_ANIM_PARAM);
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
    }
}
