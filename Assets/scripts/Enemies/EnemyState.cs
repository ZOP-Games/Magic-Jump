using System.Collections.Generic;
using GameExtensions.Debug;
using UnityEngine;

namespace GameExtensions.Enemies
{
    public abstract class EnemyState : State
    {
        private const float LookAtWeight = 0.1f;
        private const float LookAtRadius = 1;
        private readonly EnemyStateManager enemy;

        protected EnemyState(EnemyStateManager enemy) : base(enemy)
        {
            this.enemy = enemy;
        }

        protected void LookAtMe(Transform target)
        {
            enemy.Ctg.AddMember(target, LookAtWeight, LookAtRadius);
        }

        protected void DontLookAtMe(Transform target)
        {
            enemy.Ctg.RemoveMember(target);
        }
    }
}