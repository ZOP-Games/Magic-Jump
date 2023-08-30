using Cinemachine;
using GameExtensions.Debug;
using TMPro;
using UnityEngine;

namespace GameExtensions.Enemies
{
    /// <summary>
    ///     Base class for all enemies.
    /// </summary>
    [RequireComponent(typeof(LODGroup))]
    public abstract class EnemyBase : Entity
    {
        protected const float TrackInterval = .02f;
        private const float LookAtWeight = 0.1f;
        private const float LookAtRadius = 1;
        protected TextMeshPro hpText;
        protected abstract float Height { get; }
        protected abstract byte XpReward { get; }
        protected abstract Transform PlayerTransform { get; set; }
        protected abstract float AtkRepeatRate { get; }
        protected abstract CinemachineTargetGroup Ctg { get; set; }

        protected new void Start()
        {
            base.Start();
            tag = "Enemy";
            ApplyDifficulty();
            Difficulty.DifficultyLevelChanged += ApplyDifficulty;
        }

        //death logic, rewards the player with xp, removes focus and returns to the pool
        public override void Die()
        {
            Player.Instance.AddXp(XpReward);
            DontLookAtMe(transform);
            StopAiming();
            Destroy(GetComponentInChildren<EnemyLocation>().gameObject);
        }

        public void Reset()
        {
            Hp = 100;
        }

        protected virtual void Aim()
        {
            var tf = transform;
            var pos = PlayerTransform.position;

            if (Mathf.Abs(Vector3.Distance(tf.position, pos)) > AtkRange)
            {
                tf.LookAt(new Vector3(pos.x, tf.position.y, pos.z));
                CancelInvoke(nameof(Attack));
                var fw = tf.forward.normalized * TrackInterval;
                fw.y = 0;
                Move(fw);
            }
            else if (!IsInvoking(nameof(Attack)))
            {
               InvokeRepeating(nameof(Attack),0.5f,AtkRepeatRate);
               //isAttacking = true;
            }

        }

        protected override void Attack()
        {
            anim.SetTrigger(AttackingPmHash);
            Player.Instance.TakeDamage(AtkPower);
            //isAttacking = false;
        }

        protected void StopAiming()
        {
            CancelInvoke(nameof(Attack));
            anim.SetBool(MovingPmHash, false);
            anim.SetBool(RunningPmHash, false);
            CancelInvoke(nameof(Aim));
        }

        protected void LookAtMe(Transform target)
        {
            Ctg.AddMember(target, LookAtWeight, LookAtRadius);
        }

        protected void DontLookAtMe(Transform target)
        {
            Ctg.RemoveMember(target);
        }

        private void ApplyDifficulty()
        {
            Hp = Mathf.RoundToInt(Hp * DifficultyMultiplier);
            AtkPower = Mathf.RoundToInt(AtkPower * DifficultyMultiplier);
            Defense = Mathf.RoundToInt(Defense * DifficultyMultiplier);
        }

        private void OnDisable()
        {
            StopAiming();
        }
    }
}