using System;
using Cinemachine;
using UnityEngine;

namespace GameExtensions.Enemies
{
    public class EnemyStateManager : StateManager
    {
        public static EnemyAimState AimState { get; private set; }
        public static EnemyIdleState IdleState { get; private set; }

        protected const int StunTime = 3;

        public CinemachineTargetGroup Ctg { get; private set; }
        public Animator Animator { get; private set; }
        private float DifficultyMultiplier { get; } = Difficulty.DifficultyMultiplier;


        private void Start()
        {
            #region StateConstruction

            AimState ??= new EnemyAimState(this);
            IdleState ??= new EnemyIdleState(this);

            #endregion

            tag = "Enemy";
            ApplyDifficulty();
            Difficulty.DifficultyLevelChanged += ApplyDifficulty;
            Ctg = FindAnyObjectByType<CinemachineTargetGroup>();
            Animator = GetComponent<Animator>();
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