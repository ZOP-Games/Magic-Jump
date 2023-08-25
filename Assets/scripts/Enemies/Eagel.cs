using Cinemachine;
using TMPro;
using UnityEngine;

namespace GameExtensions.Enemies
{
    /// <summary>
    ///     <inheritdoc cref="Kwork" />
    /// </summary>
    public class Eagel : EnemyBase
    {
        //we'll put navigation, animations + other things that are unique to this kind of enemy (the bird)

        //setting Entity properties, for more info -> see Entity
        protected override int AttackingPmHash => Animator.StringToHash("attacking");
        protected override int MovingPmHash => Animator.StringToHash("moving");
        protected override int RunningPmHash => Animator.StringToHash("running");
        protected override int DamagePmHash => Animator.StringToHash("damage");
        protected override Vector3 AtkSpherePos => Vector3.zero;
        protected override int AtkSphereRadius => 6;
        protected override float Height => 1.5f;
        protected override byte XpReward => 12;
        protected override Transform PlayerTransform { get; set; }
        protected override float AtkRepeatRate => 1.5f;
        protected override CinemachineTargetGroup Ctg { get; set; }

        protected new void Start()
        {
            //setting the attack stat for the enemy and getting some components from the gameobject
            AtkRange = 10;
            AtkPower = 1;
            rb = GetComponent<Rigidbody>();
            anim = GetComponent<Animator>();
            hpText = GetComponentInChildren<TextMeshPro>();
            hpText.SetText("HP: 100");
            Ctg = FindObjectOfType<CinemachineTargetGroup>();
            void GetPlayerTransform(){
                PlayerTransform = Player.Instance.transform;
            }
            if(Player.Instance is not null) GetPlayerTransform();
            else Invoke(nameof(GetPlayerTransform),1);
            HealthChanged += () => hpText.SetText("HP: " + Hp);
            base.Start();
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
            tf.LookAt(pos);
            if (Mathf.Abs(Vector3.Distance(tf.position, pos)) > AtkRange)
            {
                CancelInvoke(nameof(Attack));
                isAttacking = false;
                anim.SetBool(MovingPmHash, true);
                Move(tf.InverseTransformDirection(tf.forward.normalized*0.005f));
            }
            else if (!isAttacking)
            {
                InvokeRepeating(nameof(Attack), 0, AtkRepeatRate);
                isAttacking = true;
            }
        }
    }
}