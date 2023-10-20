using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GameExtensions
{
    public abstract class EntityStateManager : StateManager, IMediator
    {
        protected const int WalkSpeed = 5;
        protected const int RunSpeed = 15;
        protected const int MoveForceMultiplier = 25;
        protected const int StunTime = 3;
        public float atkRange;
        [SerializeField] private int hp = 100;

        [field: SerializeField] public int AtkPower { get; set; } = 10;

        [field: SerializeField] public int Defense { get; set; } = 10;
        public State DeathState { get; protected set; }
        public State IdleState { get; protected set; }
        public State DamageState { get; private set; }


        public int Hp
        {
            get => hp;
            set
            {
                hp = value;
                HealthChanged?.Invoke();
            }
        }

        protected void Start()
        {
            #region StateConstruction

            DamageState = new DamageState(this);

            #endregion
        }

        public void Mediate<T>(NotifyableState<T> state, T value)
        {
            state.Notify(value);
            SetState(state);
        }

        public event UnityAction HealthChanged;
    }
}