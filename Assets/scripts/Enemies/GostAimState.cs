using GameExtensions.Debug;
using UnityEngine;

namespace GameExtensions.Enemies
{
    public class GostAimState : EnemyAimState
    {
        private const int HeightMultiplier = 3;
        private Transform tf;
        private Transform playerTf;
        private Vector3 lookAtPosition;

        public GostAimState(EnemyStateManager enemy) : base(enemy)
        {
        }

        public override void Start()
        {
            base.Start();
            tf = context.transform;
            playerTf = Player.Instance.transform;
            inRange = false;
            lookAtPosition = new Vector3(0,0,0);
        }

        public override void FixedUpdate()
        {
            var playerPos = playerTf.position;
            var position = tf.position;
            var distance = Vector3.Distance(position, playerPos);
            lookAtPosition.Set(playerPos.x,position.y,playerPos.z);
            if (distance > enemy.atkRange)
            {
                if(Mathf.Sign(tf.eulerAngles.x) < 30)
                {
                    tf.LookAt(lookAtPosition);
                }

                var dir = tf.forward;
                dir.y -= Mathf.Log10(distance) * HeightMultiplier;
                dir *= Time.fixedDeltaTime;
                Move(dir);
                DebugConsole.Log("Moving differently in direction: " + dir);
            }
            else inRange = true;

            CheckForTransition();
        }
    }
}