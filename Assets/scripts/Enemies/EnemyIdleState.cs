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

        protected override void CheckForTransition()
        {
            if(canAim) context.SetState(EnemyStateManager.AimState);
        }

        public override void OnTriggerEnter(Collider collider)
        {
            if (!collider.CompareTag("Player")) return;
            LookAtMe(context.transform);
            canAim = true;
            CheckForTransition();
        }
    }
}