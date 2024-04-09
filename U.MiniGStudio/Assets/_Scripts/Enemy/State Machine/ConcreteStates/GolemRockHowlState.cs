using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiniGStudio
{
    public class GolemRockHowlState : EnemyState
    {
        [System.Serializable]
        public struct Descriptor
        {
            public Rock RockPrefab;
            public int RockCount;
            public float SpawnRadius;
            public Vector3 Center;
            public float RockDistance;
            public float RockLifespan;
            public float ElevationDuration;
        }

        public List<Rock> Rocks = new List<Rock>();
        public List<Vector3> rockPositions = new List<Vector3>();

        public Descriptor Desc { get; private set; }

        private int _rockHowlHash;

        public GolemRockHowlState(Enemy enemy, EnemyStateMachine enemyStateMachine, Descriptor desc) : base(enemy, enemyStateMachine)
        {
            Desc = desc;
            _rockHowlHash = Animator.StringToHash("RockHowl");
        }

        public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
        {
            base.AnimationTriggerEvent(triggerType);
            switch(triggerType)
            {
                case Enemy.AnimationTriggerType.RockHowlBegin:
                    SpawnRocks();
                    break;
                case Enemy.AnimationTriggerType.RockHowlEnd:
                    ChangeToChaseState();
                    break;
                default:
                    break;
            }
        }

        public override void EnterState()
        {
            base.EnterState();
            _enemy.Animator.SetTrigger(_rockHowlHash);
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

        public void SpawnRocks()
        {
            for (int i = 0; i < Desc.RockCount; i++)
            {
                Vector3 randomPos = Helpers.RandomPointInCircle(Desc.Center, Desc.SpawnRadius);
                if (IsPositionValid(randomPos))
                {
                    Rock newRock = (Rock)GameObject.Instantiate(Desc.RockPrefab, randomPos, Quaternion.identity);
                    rockPositions.Add(randomPos);
                    Rocks.Add(newRock);
                    newRock.BindWithGolem(this);
                }
            }
        }

        public void ChangeToChaseState()
        {
            _enemyStateMachine.ChangeState(_enemy.ChaseState);
        }

        private bool IsPositionValid(Vector3 position)
        {
            foreach (Vector3 rockPos in rockPositions)
            {
                if (Vector3.Distance(position, rockPos) < Desc.RockDistance)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
