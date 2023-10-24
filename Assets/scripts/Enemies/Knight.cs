using Cinemachine;
using GameExtensions.Debug;
using TMPro;
using UnityEngine;

namespace GameExtensions.Enemies
{
    /// <summary>
    ///     A Knight (lovag) enemy.
    /// </summary>
    public class Knight : EnemyBase
    {
        private float height;
        private bool attackToggle;

        protected override int AttackingPmHash => Animator.StringToHash("attack");
        protected int Attacking2PmHash => Animator.StringToHash("attack2");
        protected override int MovingPmHash => Animator.StringToHash("moving");
        protected override int RunningPmHash => Animator.StringToHash("running");
        protected override int DamagePmHash => Animator.StringToHash("damage");
        protected override Vector3 AtkSpherePos => Vector3.forward;
        protected override int AtkRange => 3;
        protected override float Height => height;
        protected override byte XpReward => 20;
        protected override Transform PlayerTransform { get; set; }
        protected override float AtkRepeatRate => 4.08f;
        protected override CinemachineTargetGroup Ctg { get; set; }

        private new void Start()
        {
            base.Start();
            height = transform.position.y * 2;
            AtkPower = 10;
            AttackDelay = 3f;
            GetComponent<Rigidbody>();
            anim = GetComponent<Animator>();
            cc = GetComponent<CharacterController>();
            hpText = GetComponentInChildren<TextMeshPro>();
            if (hpText is null)
            {
                DebugConsole.Log("Where yo HP text at for " + name + "??", DebugConsole.WarningColor);
            }
            else
            {
                hpText.SetText("HP: 100");
                HealthChanged += () => hpText.SetText("HP: " + Hp);
            }

            Ctg = FindAnyObjectByType<CinemachineTargetGroup>();
            if (GetComponentInChildren<Collider>() is null)
                DebugConsole.LogError("The attack trigger on " + name + " missing u idoit");
            if (Player.Instance is not null) GetPlayerTransform();
            else Invoke(nameof(GetPlayerTransform), 1);
            return;

            void GetPlayerTransform()
            {
                PlayerTransform = Player.Instance.transform;
                if (PlayerTransform is null) DebugConsole.LogError("Can't find PlayerTrandform anywhere!");
            }
        }

        protected override void Attack()
        {
            if (attackToggle)
            {
                anim.SetTrigger(AttackingPmHash);
                attackToggle = false;
            }
            else
            {
                anim.SetTrigger(Attacking2PmHash);
                attackToggle = true;
            }
            Player.Instance.TakeDamage(AtkPower);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            LookAtMe(transform);
            InvokeRepeating(nameof(Aim), 0, TrackInterval);
        }

        //if the player leaves the aim trigger, it stops the Check coroutine
        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            DontLookAtMe(transform);
            StopAiming();
        }
    }
}