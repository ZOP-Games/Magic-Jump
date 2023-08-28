using GameExtensions.Debug;
using System.Linq;
using Cinemachine;
using TMPro;
using UnityEngine;

namespace GameExtensions.Enemies
{
    /// <summary>
    ///     A Kwork enemy.
    /// </summary>
    public class Kwork : EnemyBase
    {
        private readonly WaitForSeconds wfs = new(0.5f);

        //class for the Kwork enemy, it's a basic implementation of EnemyBase
        protected override int AttackingPmHash => Animator.StringToHash("attack");
        protected override int MovingPmHash => Animator.StringToHash("moving");
        protected override int RunningPmHash => Animator.StringToHash("running");
        protected override int DamagePmHash => Animator.StringToHash("damage");
        protected override Vector3 AtkSpherePos => Vector3.zero;
        protected override int AtkSphereRadius => 3;
        protected override Transform PlayerTransform { get; set; }
        protected override float Height => 3;
        protected override byte XpReward => 12;
        protected override float AtkRepeatRate => 2f;
        protected override CinemachineTargetGroup Ctg { get; set; }

        protected new void Start()
        {
            //setting the attack stat for the enemy and getting some components from the gameobject
            base.Start();
            AtkRange = 4;
            AtkPower = 1;
            rb = GetComponent<Rigidbody>();
            anim = GetComponent<Animator>();
            cc = GetComponent<CharacterController>();
            hpText = GetComponentInChildren<TextMeshPro>();
            if (hpText is null)
            {
                DebugConsole.Log("Where yo HP text at for " + name + "???",DebugConsole.WarningColor);
            }
            else
            {
                hpText.SetText("HP: 100");
                HealthChanged += () => hpText.SetText("HP: " + Hp);
            }

            Ctg = FindObjectOfType<CinemachineTargetGroup>();
            if (GetComponentsInChildren<Collider>()
                .All(c => !c.isTrigger))
                DebugConsole.LogError("The attack trigger on " + name + " is missing u idoit");
            void GetPlayerTransform(){
                PlayerTransform = Player.Instance.transform;
            }
            if(Player.Instance is not null) GetPlayerTransform();
            else Invoke(nameof(GetPlayerTransform),1);
        }

        private void OnTriggerEnter(Collider other) {
            if(!other.CompareTag("Player")) return;
            UnityEngine.Debug.Log("I see trigger",other);
            LookAtMe(transform);
            InvokeRepeating(nameof(Aim), 0, TrackInterval); 
        }
        
        private void OnControllerColliderHit(ControllerColliderHit hit) {
            if(!hit.collider.CompareTag("Player")) return;
            DebugConsole.Log("I feel player");
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