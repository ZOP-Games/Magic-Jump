using Cinemachine;
using GameExtensions.Debug;
using TMPro;
using UnityEngine;

namespace GameExtensions.Enemies
{
    /// <summary>
    ///     An Gost (szellem) enemy.
    /// </summary>
    public class Gost : EnemyBase
    {
        private const float FlightFactor = 15f;

        //setting Entity properties, for more info -> see Entity
        protected override int AttackingPmHash => Animator.StringToHash("attack");
        protected override int MovingPmHash => Animator.StringToHash("moving");
        protected override int RunningPmHash => Animator.StringToHash("running");
        protected override int DamagePmHash => Animator.StringToHash("damage");
        protected override Vector3 AtkSpherePos => Vector3.zero;
        protected override int AtkRange => 3;
        protected override float Height => 1.5f;
        protected override byte XpReward => 12;
        protected override Transform PlayerTransform { get; set; }
        protected override float AtkRepeatRate => 2.11f;
        protected override CinemachineTargetGroup Ctg { get; set; }

        protected new void Start()
        {
            base.Start();
            //setting the attack stat for the enemy and getting some components from the gameobject
            AtkPower = 1;
            //AttackDelay = 0.5f;
            GetComponent<Rigidbody>();
            anim = GetComponent<Animator>();
            hpText = GetComponentInChildren<TextMeshPro>();
            hpText.SetText("HP: 100");
            Ctg = FindAnyObjectByType<CinemachineTargetGroup>();
            cc = GetComponent<CharacterController>();
            void GetPlayerTransform()
            {
                PlayerTransform = Player.Instance.transform;
            }
            if (Player.Instance is not null) GetPlayerTransform();
            else Invoke(nameof(GetPlayerTransform), 1);
            HealthChanged += () => hpText.SetText("HP: " + Hp);

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

        protected override void Aim()
        {
            var tf = transform;
            var pos = PlayerTransform.position;
            if (Mathf.Abs(Vector3.Distance(tf.position, pos)) > AtkRange)
            {
                CancelInvoke(nameof(Attack));
                anim.SetBool(MovingPmHash, true);
                tf.LookAt(pos);
                var dir = tf.forward;
                var rnd = Random.Range(0, 1f);
                if (rnd > 0.7f) dir.y += FlightFactor * TrackInterval;
                else if (rnd < 0.3f && !cc.isGrounded) dir.y -= FlightFactor * TrackInterval;
                Move(dir * TrackInterval);
            }
            else if (!IsInvoking(nameof(Attack)))
            {
                InvokeRepeating(nameof(Attack), AttackDelay, AtkRepeatRate);
            }
        }
    }
}