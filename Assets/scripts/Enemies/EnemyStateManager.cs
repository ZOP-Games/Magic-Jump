using Cinemachine;
using UnityEngine;

namespace GameExtensions.Enemies
{
    public class EnemyStateManager : EntityStateManager
    {
        public static EnemyAimState AimState { get; private set; }
        public static EnemyIdleState IdleState { get; private set; }
        public static EnemyAttackState AttackState { get; private set; }

        private float DifficultyMultiplier { get; } = Difficulty.DifficultyMultiplier;
        [SerializeField] private float attackWait;
        [SerializeField] private float attackRepeat;
        [SerializeField] private int xpReward;

        private void Start()
        {
            #region StateConstruction

            AimState ??= new EnemyAimState(this);
            IdleState ??= new EnemyIdleState(this);
            AttackState ??= new EnemyAttackState(this, attackWait, attackRepeat);
            DeathState ??= new EnemyDeathState(this, xpReward);

            #endregion

            tag = "Enemy";
            ApplyDifficulty();
            Difficulty.DifficultyLevelChanged += ApplyDifficulty;
            SetState(IdleState);
        }

        protected new void OnDisable()
        {
            base.OnDisable();
            FindAnyObjectByType<CinemachineTargetGroup>().RemoveMember(transform);
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
            SetState(IdleState);
        }
    }
}