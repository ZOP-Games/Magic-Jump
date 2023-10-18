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

        public new void Start()
        {
            DebugConsole.Log("Here I am (again?)");
            base.Start();
            canAim = false;
        }

        protected override void CheckForTransition()
        {
            if(canAim) context.SetState(EnemyStateManager.AimState);
        }

        public override void OnTriggerEnter(Collider collider)
        {
            if (!collider.CompareTag("Player")) return;
            DebugConsole.Log("I see you");
            LookAtMe();
            canAim = true;
            CheckForTransition();
        }
    }
}