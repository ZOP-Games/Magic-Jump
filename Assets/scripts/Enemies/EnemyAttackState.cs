using System.Collections;
using UnityEngine;

namespace GameExtensions.Enemies
{
    public class EnemyAttackState : EnemyState
    {
        private readonly WaitForSeconds wait;
        private readonly WaitForSeconds repeat;
        private int atkPower;
        private bool canAttack = true;
        private Transform tf;
        private Transform playerTf;
        private Animator anim;
        private int attackHash;

        public EnemyAttackState(EnemyStateManager enemy, float waitInterval, float repeatInterval) : base(enemy)
        {
            wait = new WaitForSeconds(waitInterval);
            repeat = new WaitForSeconds(repeatInterval);
        }

        protected override void CheckForTransition()
        {
            if (!canAttack) context.SetState(EnemyStateManager.IdleState);
        }

        public override void Start()
        {
            anim = enemy.GetComponent<Animator>();
            atkPower = enemy.AtkPower;
            tf = context.transform;
            playerTf = Player.Instance.transform;
            attackHash = Animator.StringToHash("attack");
            context.StartCoroutine(Coroutine());
        }

        public override void ExitState()
        {
            context.StopCoroutine(nameof(Coroutine));
        }

        private IEnumerator Coroutine()
        {
            while (true)
            {
                anim.SetTrigger(attackHash);
                yield return wait;
                Player.Instance.TakeDamage(atkPower);
                if (Mathf.Abs(Vector3.Distance(tf.position, playerTf.position)) > enemy.atkRange)
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