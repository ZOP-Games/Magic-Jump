using System;
using GameExtensions.Debug;
using UnityEngine;
using UnityEngine.Events;

namespace GameExtensions
{
    public abstract class EntityStateManager : StateManager
    {
        public static State DeathState { get; protected set; }
        public static State DamageState { get; private set; }
        public byte atkRange;
        [field: SerializeField] public int Hp { get; set; } = 100;

        [field: SerializeField] public int AtkPower { get; set; } = 10;

        [field: SerializeField] public int Defense { get; set; } = 10;

        protected const int WalkSpeed = 5;
        protected const int RunSpeed = 15;
        protected const int MoveForceMultiplier = 25;
        protected const int StunTime = 3;

        private void Start()
        {
            #region StateConstruction


            DamageState = new DamageState(this);

            #endregion
        }
    }
}