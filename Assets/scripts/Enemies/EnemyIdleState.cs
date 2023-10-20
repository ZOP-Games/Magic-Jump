using System.Collections.Generic;
using GameExtensions.Debug;
using UnityEngine;

namespace GameExtensions.Enemies
{
    public class EnemyIdleState : EnemyState
    {
        private bool canAim;

        public EnemyIdleState(EnemyStateManager context) : base(context)
        {
        }

        public override void Start()
        {
            base.Start();
            canAim = false;
        }

        protected override void CheckForTransition()
        {
            if (canAim) context.SetState(enemy.AimState);
        }

        public override void OnTriggerEnter(Collider collider)
        {
            if (!collider.CompareTag("Player") || canAim) return;
            DebugConsole.Log("I see you");
            LookAtMe();
            canAim = true;
            CheckForTransition();
        }
    }
}