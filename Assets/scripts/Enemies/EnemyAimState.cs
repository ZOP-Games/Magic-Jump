using Cinemachine;
using GameExtensions.Debug;
using UnityEngine;

namespace GameExtensions.Enemies
{
    public class EnemyAimState : EnemyState
    {
        private const float TrackInterval = .02f;
        private const float MoveMultiplier = 0.04f;
        private const int MoveForceMultiplier = 25;
        private const float GravityForce = 0.98f;

        protected readonly int movingPmHash = Animator.StringToHash("moving");
        protected readonly int runningPmHash = Animator.StringToHash("running");

        private Transform playerTransform;
        private bool inRange;
        private bool isPassive;
        private Animator anim;
        private CharacterController cc;

        protected override void CheckForTransition()
        {
            if (inRange) context.SetState(EnemyStateManager.AttackState);
            else if (isPassive) context.SetState(EnemyStateManager.IdleState);
            //todo:fix transitions
        }

        public override void Start()
        {
            playerTransform = Player.Instance.transform;
            anim = enemy.GetComponent<Animator>();
            cc = enemy.GetComponent<CharacterController>();
            ctg = Object.FindAnyObjectByType<CinemachineTargetGroup>();
        }

        public override void Update()
        {
            var tf = context.transform;
            var pos = playerTransform.position;
            if (Mathf.Abs(Vector3.Distance(tf.position, pos)) > enemy.atkRange)
            {
                inRange = false;
                tf.LookAt(new Vector3(pos.x, tf.position.y, pos.z));

                var fw = Time.deltaTime * TrackInterval * tf.forward;
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
            DontLookAtMe(context.transform);
            isPassive = false;
        }

        public override void ExitState()
        {
            anim.SetBool(movingPmHash, false);
        }

        public void Move(Vector3 direction, bool isRunning = false, int maxSpeed = 5)
        {
            anim.SetBool(movingPmHash, true);
            anim.SetBool(runningPmHash, isRunning);

            var moveDirection = new Vector3(
                direction.x * maxSpeed * MoveForceMultiplier,
                cc.velocity.y - GravityForce * Time.deltaTime,
                direction.z * maxSpeed * MoveForceMultiplier);
            cc.Move(moveDirection);
        }

        public EnemyAimState(EnemyStateManager enemy) : base(enemy)
        {

        }
    }
}