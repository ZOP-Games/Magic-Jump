using System.Collections;
using UnityEngine;

namespace GameExtensions.Enemies
{
    public class EnemyAttackState : EnemyState
    {
        private WaitForSeconds wait;
        private WaitForSeconds repeat;
        private int atkPower;
        private bool canAttack = true;
        private Transform tf;
        private Transform playerTf;

        public EnemyAttackState(EnemyStateManager enemy,int waitInterval,int repeatInterval) : base(enemy)
        {
            wait = new WaitForSeconds(waitInterval);
            repeat = new WaitForSeconds(repeatInterval);
        }

        protected override void CheckForTransition()
        {
            if (!canAttack) context.SetState(EnemyStateManager.AimState);
        }

        public override void Start()
        {
            atkPower = context.AtkPower;
            context.StartCoroutine(nameof(Coroutine));
            tf = context.transform;
            playerTf = Player.Instance.transform;
        }

        public override void ExitState()
        {
            context.StopCoroutine(nameof(Coroutine));
        }

        private IEnumerator Coroutine()
        {
            while (true)
            {
                yield return wait;
                Player.Instance.TakeDamage(atkPower);
                if (Mathf.Abs(Vector3.Distance(tf.position, playerTf.position)) > context.atkRange)
                {
                    canAttack = false;
                    break;
                }
                CheckForTransition();
                yield return repeat;
            }
        }
    }
}