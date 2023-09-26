using System;
using Cinemachine;
using UnityEngine;

namespace GameExtensions.Enemies
{
    public class EnemyStateManager : EntityStateManager
    {
        public static EnemyAimState AimState { get; private set; }
        public static EnemyIdleState IdleState { get; private set; }
        public static EnemyAttackState AttackIdle { get; private set; }

        private float DifficultyMultiplier { get; } = Difficulty.DifficultyMultiplier;
        [SerializeField] private float attackWait;
        [SerializeField] private float attackRepeat;

        private void Start()
        {
            #region StateConstruction

            AimState ??= new EnemyAimState(this);
            IdleState ??= new EnemyIdleState(this);
            AttackIdle ??= new EnemyAttackState(this, attackWait, attackRepeat);

            #endregion

            tag = "Enemy";
            ApplyDifficulty();
            Difficulty.DifficultyLevelChanged += ApplyDifficulty;
            SetState(IdleState);
        }

        private void ApplyDifficulty()
        {
            Hp = Mathf.RoundToInt(Hp * DifficultyMultiplier);
            AtkPower = Mathf.RoundToInt(AtkPower * DifficultyMultiplier);
            Defense = Mathf.RoundToInt(Defense * DifficultyMultiplier);
        }

        public void Reset()
        {
            Hp = 100;
        }
    }
}