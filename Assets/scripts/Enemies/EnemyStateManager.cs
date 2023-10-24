using System.Collections.Generic;
using Cinemachine;
using GameExtensions.Debug;
using UnityEngine;

namespace GameExtensions.Enemies
{
    public class EnemyStateManager : EntityStateManager
    {
        private const int BaseHp = 100;
        [SerializeField] protected float attackWait;
        [SerializeField] protected float attackRepeat;
        [SerializeField] private int xpReward;
        public EnemyAimState AimState { get; protected set; }
        public EnemyAttackState AttackState { get; protected set; }

        private float DifficultyMultiplier { get; } = Difficulty.DifficultyMultiplier;

        public void Reset()
        {
            Hp = (int) (BaseHp * DifficultyMultiplier);
            if (CurrentState == IdleState) return;
            SetState(IdleState);
        }

        protected new void Start()
        {
            base.Start();

            #region StateConstruction

            AimState ??= new EnemyAimState(this);
            IdleState ??= new EnemyIdleState(this);
            AttackState ??= new EnemyAttackState(this, attackWait, attackRepeat);
            DeathState ??= new EnemyDeathState(this, xpReward);

            #endregion

            tag = "Enemy";
            ApplyDifficulty();
            Difficulty.DifficultyLevelChanged += ApplyDifficulty;
            if (CurrentState == IdleState) return;
            SetState(IdleState);
        }

        protected new void OnDisable()
        {
            if (CurrentState is null) return;
            FindAnyObjectByType<CinemachineTargetGroup>().RemoveMember(transform);
            base.OnDisable();
        }

        private void ApplyDifficulty()
        {
            Hp = Mathf.RoundToInt(BaseHp * DifficultyMultiplier);
            AtkPower = Mathf.RoundToInt(AtkPower * DifficultyMultiplier);
            Defense = Mathf.RoundToInt(Defense * DifficultyMultiplier);
        }
    }
}