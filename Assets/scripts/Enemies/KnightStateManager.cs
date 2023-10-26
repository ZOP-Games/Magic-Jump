using UnityEngine;

namespace GameExtensions.Enemies
{
    public sealed class KnightStateManager : EnemyStateManager
    {
        [SerializeField] private float attackWait2;
        [SerializeField] private float attackRepeat2;

        private new void Start()
        {
            base.Start();
            #region StateConstruction

            AttackState = new KnightAttackState(this, attackWait, attackRepeat, attackWait2, attackRepeat2);

            #endregion
        }

        private new void OnDisable()
        {
            base.OnDisable();
        }
    }
}