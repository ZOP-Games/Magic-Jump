using System.Collections.Generic;
using Cinemachine;
using GameExtensions.Debug;
using UnityEngine;

namespace GameExtensions.Enemies
{
    public abstract class EnemyState : State
    {
        private const float LookAtWeight = 0.1f;
        private const float LookAtRadius = 1;
        protected readonly EnemyStateManager enemy;
        protected CinemachineTargetGroup ctg;

        protected EnemyState(EnemyStateManager enemy) : base(enemy)
        {
            this.enemy = enemy;
        }

        public override void Start()
        {
            ctg = Object.FindAnyObjectByType<CinemachineTargetGroup>();
        }

        protected void LookAtMe(Transform target)
        {
            ctg.AddMember(target, LookAtWeight, LookAtRadius);
        }

        protected void DontLookAtMe(Transform target)
        {
            ctg.RemoveMember(target);
        }
    }
}