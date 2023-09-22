using GameExtensions.Debug;
using UnityEngine;

namespace GameExtensions.Enemies
{
    public class EnemyAimState : State
    {
        private const float TrackInterval = .02f;
        private const float MoveMultiplier = 0.04f;

        private Transform playerTransform;
        private bool inRange;

        protected override void CheckForTransition()
        {
            if(inRange) context.SetState(EnemyStateManager.AimState);
        }

        public override void Start()
        {
            playerTransform = Player.Instance.transform;
        }

        public override void Update()
        {
            var tf = context.transform;
            var pos = playerTransform.position;
            if (Mathf.Abs(Vector3.Distance(tf.position, pos)) > context.atkRange)
            {
                inRange = false;
                tf.LookAt(new Vector3(pos.x, tf.position.y, pos.z));
                
                var fw = MoveMultiplier * TrackInterval * tf.forward;
                fw.y = 0;
                //Move(fw);
                DebugConsole.Log("Moving in direction: " + fw);
            }
            else
            {
                inRange = true;
            } 
            CheckForTransition();
        }

        

        public EnemyAimState(StateManager context) : base(context)
        {

        }
    }
}