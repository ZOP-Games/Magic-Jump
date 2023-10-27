using System.Collections.Generic;
using Cinemachine;
using GameExtensions.Debug;
using UnityEngine;

namespace GameExtensions.Enemies
{
    public class EnemyAimState : EnemyState
    {
        protected const float MoveForceMultiplier = .5f;
        private const float GravityForce = 0.98f;

        protected readonly int movingPmHash = Animator.StringToHash("moving");
        protected readonly int runningPmHash = Animator.StringToHash("running");
        protected Animator anim;
        protected CharacterController cc;
        protected bool inRange;
        protected bool isPassive;

        private Transform playerTransform;
        private Transform tf;

        public EnemyAimState(EnemyStateManager enemy) : base(enemy)
        {
        }

        protected override void CheckForTransition()
        {
            if (inRange) context.SetState(enemy.AttackState);
            else if (isPassive) context.SetState(enemy.IdleState);
        }

        public override void Start()
        {
            base.Start();
            isPassive = false;
            inRange = false;
            playerTransform = Player.Instance.transform;
            anim = enemy.GetComponent<Animator>();
            cc = enemy.GetComponent<CharacterController>();
            ctg = Object.FindAnyObjectByType<CinemachineTargetGroup>();
            tf = context.transform;
        }

        public override void FixedUpdate()
        {
            var pos = playerTransform.position;
            if (Vector3.Distance(tf.position, pos) > enemy.atkRange)
            {
                inRange = false;
                tf.LookAt(new Vector3(pos.x, tf.position.y, pos.z));

                var fw = tf.forward * Time.fixedDeltaTime;
                fw.y = 0;
                Move(fw);
                DebugConsole.Log("Moving in direction: " + fw);
            }
            else
            {
                inRange = true;
            }

            CheckForTransition();
        }

        public override void OnTriggerExit(Collider collider)
        {
            if (!collider.CompareTag("Player")) return;
            DontLookAtMe();
            isPassive = true;
        }

        public override void ExitState()
        {
            anim.SetBool(movingPmHash, false);
        }

        protected virtual void Move(Vector3 direction, bool isRunning = false, int maxSpeed = 5)
        {
            anim.SetBool(movingPmHash, true);
            anim.SetBool(runningPmHash, isRunning);

            var moveDirection = new Vector3(
                direction.x * maxSpeed * MoveForceMultiplier,
                (direction.y - GravityForce) * MoveForceMultiplier,
                direction.z * maxSpeed * MoveForceMultiplier);
            cc.Move(moveDirection);
        }
    }
}