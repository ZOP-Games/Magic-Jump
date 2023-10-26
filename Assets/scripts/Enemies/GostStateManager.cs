using UnityEngine;

namespace GameExtensions.Enemies
{
    public sealed class GostStateManager : EnemyStateManager
    {
        private new void Start()
        {
            base.Start();
            #region StateConstruction
            AimState = new GostAimState(this);
            #endregion
        }
    }
}