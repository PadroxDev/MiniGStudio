using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiniGStudio
{
    [System.Serializable]
    public struct GolemRockHowlDesc
    {
        public Rock RockPrefab;
        public int RockCount;
        public float SpawnRadius;
        public Vector3 Center;
        public float RockDistance;
        public float RockLifespan;
        public float ElevationDuration;
    }
        
    public class GolemRockHowlState : EnemyState
    {
        public GolemRockHowlDesc Desc { get; private set; }

        public List<Rock> Rocks = new List<Rock>();
        public List<Vector3> rockPositions = new List<Vector3>();

        public GolemRockHowlState(Enemy enemy, EnemyStateMachine enemyStateMachine, GolemRockHowlDesc stateDesc) : base(enemy, enemyStateMachine)
        {
            Desc = stateDesc;
        }

        public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
        {
            base.AnimationTriggerEvent(triggerType);
        }

        public override void EnterState()
        {
            base.EnterState();
            SpawnRocks();
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
