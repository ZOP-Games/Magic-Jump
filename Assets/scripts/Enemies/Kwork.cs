using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
namespace GameExtensions.Enemies
{
    /// <summary>
    /// A Kwork enemy.
    /// </summary>
    public class Kwork : EnemyBase
    {
        //class for the Kwork enemy, it's a basic implementation of EnemyBase
        protected override int AttackingPmHash => Animator.StringToHash("Attack");
        protected override int MovingPmHash => Animator.StringToHash("Running");
        protected override Vector3 AtkSpherePos => Vector3.zero;
        protected override int AtkSphereRadius => 3;
        protected override Vector3 ForwardDirection => Vector3.forward;
        protected override Transform PlayerTransform { get; set; }
        protected override float Height => 3;
        protected override byte XpReward => 12;
        protected override float AtkRepeatRate => 0.5f;
        protected override CinemachineTargetGroup Ctg { get; set; }
        private readonly WaitForSeconds wfs = new(0.5f);

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


        protected new void Start()
        {
            //setting the attack stat for the enemy and getting some components from the gameobject
            AtkRange = 4;
            AtkPower = 1;
            rb = GetComponent<Rigidbody>();
            anim = GetComponent<Animator>();
            hpText = GetComponentInChildren<TextMeshPro>();
            hpText.SetText("HP: 100");
            Ctg = FindObjectOfType<CinemachineTargetGroup>();
            Player.PlayerReady += () => PlayerTransform = Player.Instance.transform;
            HealthChanged += () => hpText.SetText("HP: " + Hp);
        }
    }
}