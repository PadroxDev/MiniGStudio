using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.VFX;
using Random = UnityEngine.Random;

namespace MiniGStudio
{
    public class GolemRockHowlState : EnemyState
    {
        [System.Serializable]
        public struct Descriptor
        {
            public Rock RockPrefab;
            public Rock ChargedRockPrefab;
            public int RockCount;
            public float SpawnRadius;
            public float RockDistance;
            public float RockLifespan;
            public float ElevationDuration;
            [Range(0, 1)] public float DurationVariation;
            public float ElevationHeight;
            public float AverageSize;
            public float SizeVariation;
            [Range(0, 1)] public float ChargedRockPercentage;
            public VisualEffect GroundSmokeVFX;
            public float DissolveDuration;
            public float MinSpeedToDamage;
            public BombManager BombPrefab;
        }

        public List<Rock> Rocks = new List<Rock>();
        public List<Vector3> rockPositions = new List<Vector3>();

        public Descriptor Desc { get; private set; }

        private int _rockHowlHash;
        private Transform _rocksParent;

        public GolemRockHowlState(Enemy enemy, EnemyStateMachine enemyStateMachine, Descriptor desc) : base(enemy, enemyStateMachine)
        {
            Desc = desc;
            _rockHowlHash = Animator.StringToHash("RockHowl");
            _rocksParent = new GameObject("Rocks").transform;
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
                Vector3 center = Vector3.down * Desc.ElevationHeight;
                Vector3 randomPos = Helpers.RandomPointInCircle(center, Desc.SpawnRadius);
                if (IsPositionValid(randomPos))
                {
                    SpawnRandomRock(randomPos);
                }
            }
        }

        private void SpawnRandomRock(Vector3 pos) {
            bool charged = Random.Range(0.0f, 1.0f) <= Desc.ChargedRockPercentage;

            Rock rock = (Rock)GameObject.Instantiate(charged ? Desc.ChargedRockPrefab : Desc.RockPrefab,
                pos, Quaternion.Euler(Random.Range(-360, 360), Random.Range(-360, 360), Random.Range(-360, 360)),
                _rocksParent);
            Rocks.Add(rock);
            rockPositions.Add(pos);
            rock.BindWithGolem(this);

            Vector3 sca = Vector3.one * Desc.AverageSize;
            sca.x *= 1 + Random.Range(-Desc.ChargedRockPercentage, Desc.ChargedRockPercentage);
            sca.y *= 1 + Random.Range(-Desc.ChargedRockPercentage, Desc.ChargedRockPercentage);
            sca.z *= 1 + Random.Range(-Desc.ChargedRockPercentage, Desc.ChargedRockPercentage);
            rock.transform.localScale = sca;
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
