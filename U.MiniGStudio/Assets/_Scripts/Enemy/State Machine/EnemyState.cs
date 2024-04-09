using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MiniGStudio.Enemy;

namespace MiniGStudio
{
    public class EnemyState
    {
        protected Enemy _enemy;
        protected EnemyStateMachine _enemyStateMachine;

        public EnemyState(Enemy enemy, EnemyStateMachine enemyStateMachine)
        {
            _enemy = enemy;
            _enemyStateMachine = enemyStateMachine;
        }

        public virtual void EnterState() 
        {
            _enemy.transform.LookAt(_enemy.PlayerRB.transform.position, Vector3.up);
        }

        public virtual void ExitState() { }

        public virtual void FrameUpdate() { }

        public virtual void PhysicsUpdate() { }

        public virtual void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType) { }
    }
}
