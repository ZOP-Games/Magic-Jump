using System.Collections.Generic;
using Cinemachine;
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

        protected void LookAtMe()
        {
            ctg.AddMember(context.transform, LookAtWeight, LookAtRadius);
        }

        protected void DontLookAtMe()
        {
            ctg.RemoveMember(context.transform);
        }
    }
}