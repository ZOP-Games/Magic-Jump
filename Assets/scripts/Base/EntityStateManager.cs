using GameExtensions.Debug;
using UnityEngine;
using UnityEngine.Events;

namespace GameExtensions
{
    public abstract class EntityStateManager : StateManager, IMediator
    {
        public static State DeathState { get; protected set; }
        public static State IdleState { get; protected set; }
        public static State DamageState { get; private set; }
        public float atkRange;
        [SerializeField] private int hp = 100;
        public event UnityAction HealthChanged;

        
        public int Hp
        {
            get => hp;
            set
            {
                hp = value;
                HealthChanged?.Invoke();
            }
        }

        [field: SerializeField] public int AtkPower { get; set; } = 10;

        [field: SerializeField] public int Defense { get; set; } = 10;

        protected const int WalkSpeed = 5;
        protected const int RunSpeed = 15;
        protected const int MoveForceMultiplier = 25;
        protected const int StunTime = 3;

        public void Mediate<T>(NotifyableState<T> state, T value)
        {
            state.Notify(value);
            SetState(state);
        }

        protected void Start()
        {
            #region StateConstruction

            DamageState = new DamageState(this);

            #endregion
        }
    }
}