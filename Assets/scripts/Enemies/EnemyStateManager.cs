using UnityEngine;
using UnityEngine.Serialization;

namespace GameExtensions.Enemies
{
    public class EnemyStateManager : StateManager
    {
        public static AimState AimState { get; private set; }

        

        private void Start()
        {
            AimState ??= new AimState(this);

            
        }
    }
}