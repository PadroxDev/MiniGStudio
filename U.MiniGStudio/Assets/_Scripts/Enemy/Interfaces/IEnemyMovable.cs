using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiniGStudio
{
    public interface IEnemyMovable
    {
        Rigidbody RB { get; set; }

        void MoveEnemy(Vector3 velocity);
        void RotateEnemy(Vector3 dir);
    }
}
