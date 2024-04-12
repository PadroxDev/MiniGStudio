using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiniGStudio
{
    public class GolemChaseState : EnemyState
    {
        [System.Serializable]
        public struct Descriptor
        {
            public float ChaseSpeed;
            public float FistDetectionRange;
            public float SmashCooldown;
            public float RockCooldown;
            public float MinimumSmashDistance;
            public float MaximumRockDistance;
            public LayerMask whatisRock;
        }

        private const string CHASE_ANIM_PARAM = "Speed";

        private float _throwElapsed;
        private float _smashElapsed;

        private Descriptor _desc;

        public GolemChaseState(Enemy enemy, EnemyStateMachine enemyStateMachine, Descriptor desc) : base(enemy, enemyStateMachine)
        {
            _desc = desc;

            _smashElapsed = 0.0f;
            _throwElapsed = 0.0f;
        }

        public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
        {
            base.AnimationTriggerEvent(triggerType);
        }

        public override void EnterState()
        {
            base.EnterState();
            _enemy.Animator.SetFloat(CHASE_ANIM_PARAM, 0);
        }

        public override void ExitState()
        {
            base.ExitState();
        }

        public override void FrameUpdate()
        {
            base.FrameUpdate();

            float distance = Vector3.Distance(_enemy.PlayerRB.transform.position, _enemy.RB.transform.position);
            if (distance < _desc.FistDetectionRange)
            {
                _enemyStateMachine.ChangeState(_enemy.RockFistState);
                return;
            }

            _throwElapsed += Time.deltaTime;
            _smashElapsed += Time.deltaTime;

            if (_desc.SmashCooldown <= _smashElapsed)
            {
                if (distance >= _desc.MinimumSmashDistance)
                {
                    _smashElapsed = 0.0f;
                    _enemyStateMachine.ChangeState(_enemy.GroundSmashState);
                    return;
                }
            }

            if(_enemy.RockHowlState.Rocks.Count <= 0) {
                _enemyStateMachine.ChangeState(_enemy.RockHowlState);
                return;
            }

            Debug.Log(_throwElapsed);
            if (_desc.RockCooldown <= _throwElapsed)
            {
                Rock rock = GetClosestRock();
                if (rock != null && Vector3.Distance(_enemy.transform.position, rock.transform.position) <= _desc.MaximumRockDistance) {
                    _throwElapsed = 0.0f;
                    _enemy.RockThrowState.CurrentThrowableRock = rock;
                    _enemyStateMachine.ChangeState(_enemy.RockThrowState);
                    return;
                }
            }

            Vector3 direction = (_enemy.PlayerRB.transform.position - _enemy.RB.transform.position).normalized;
            _enemy.transform.LookAt(_enemy.PlayerRB.transform.position, Vector3.up);
            _enemy.MoveEnemy(direction * _desc.ChaseSpeed);
            _enemy.RotateEnemy(direction);
            _enemy.Animator.SetFloat(CHASE_ANIM_PARAM, 1);

        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }

        private Rock GetClosestRock()
        {
            Vector3 enemyPos = _enemy.transform.position;
            Collider[] rocks = Physics.OverlapSphere(enemyPos, 100.0f, _desc.whatisRock);
            if (rocks.Length <= 0) return null;
            GameObject rockGO = rocks[0].gameObject;

            for (int i = 1; i < rocks.Length; i++)
            {
                float dist = Vector3.Distance(rockGO.gameObject.transform.position, enemyPos);
                float nextDist = Vector3.Distance(rocks[i].gameObject.transform.position, enemyPos);
                rockGO = dist < nextDist ? rockGO : rocks[i].gameObject;
            }

            if (!rockGO.TryGetComponent(out Rock rock)) return null;
            return rock;
        }

    }
}
