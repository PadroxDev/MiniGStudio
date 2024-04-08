using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiniGStudio
{
    public class EnemyStateMachine
    {
        public EnemyState CurrentEnemyState { get; set; }

        public void Initialize(EnemyState startingState)
        {
            CurrentEnemyState = startingState;
            CurrentEnemyState.EnterState();
        }

        public void ChangeState(EnemyState newState)
        {
            CurrentEnemyState.ExitState();
            CurrentEnemyState = newState;
            CurrentEnemyState.EnterState();
        }
    }
}
