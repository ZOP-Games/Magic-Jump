using System.Linq;
using Cinemachine;
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

        //todo: research: is this necesarry??
        protected override int AttackingPmHash => Animator.StringToHash("attack");
        protected int Attacking2PmHash => Animator.StringToHash("attack2");
        protected override int MovingPmHash => Animator.StringToHash("moving");
        protected override int RunningPmHash => Animator.StringToHash("running");
        protected override int DamagePmHash => Animator.StringToHash("damage");
        protected override Vector3 AtkSpherePos => Vector3.forward;
        protected override int AtkSphereRadius => 2;
        protected override float Height => height;
        protected override byte XpReward => 20;
        protected override Transform PlayerTransform { get; set; }
        protected override float AtkRepeatRate => 0.5f;
        protected override CinemachineTargetGroup Ctg { get; set; }

        private new void Start()
        {
            base.Start();
            height = transform.position.y * 2;
            AtkRange = 4;
            AtkPower = 1;
            rb = GetComponent<Rigidbody>();
            anim = GetComponent<Animator>();
            hpText = GetComponentInChildren<TextMeshPro>();
            if (hpText is null)
            {
                Debug.LogWarning("Where yo HP text at for " + name + "??");
            }
            else
            {
                hpText.SetText("HP: 100");
                HealthChanged += () => hpText.SetText("HP: " + Hp);
            }

            Ctg = FindObjectOfType<CinemachineTargetGroup>();
            if (GetComponentsInChildren<Collider>()
                .All(c => !c.isTrigger))
                Debug.LogError("The attack trigger on " + name + " missing u idoit");
            Player.PlayerReady += () => PlayerTransform = Player.Instance.transform;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            LookAtMe(transform);
            InvokeRepeating(nameof(Aim), 0, TrackInterval);
        }

        //if the player leaves the aim trigger, it stops the Check coroutine and applies the stop aiming fix
        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            DontLookAtMe(transform);
            StopAiming();
        }
    }
}