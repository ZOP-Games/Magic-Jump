using System.Collections;
using GameExtensions.Debug;
using UnityEngine;

namespace GameExtensions.Enemies
{
    public class KnightAttackState : EnemyAttackState
    {
        private int attack1Hash;
        private int attack2Hash;

        private readonly WaitForSeconds wait2;
        private readonly WaitForSeconds repeat2;

        public KnightAttackState(EnemyStateManager enemy, float waitInterval1, float repeatInterval1,float waitInterval2, float repeatInterval2) : base(enemy, waitInterval1, repeatInterval1)
        {
            wait2 = new WaitForSeconds(waitInterval2);
            repeat2 = new WaitForSeconds(repeatInterval2);
        }

        public override void Start()
        {
            base.Start();
            base.ExitState();
            attack1Hash = Animator.StringToHash("attack");
            attack2Hash = Animator.StringToHash("attack2");
            context.StartCoroutine(AttackCoroutine());
        }

        public override void ExitState()
        {
            base.ExitState();
            context.StopAllCoroutines();
        }

        private IEnumerator AttackCoroutine()
        {
            while (canAttack)
            {
                var flip = Random.Range(0, 10);
                if (flip < 5)
                {
                    anim.SetTrigger(attack1Hash);
                    yield return wait;
                    Player.Instance.TakeDamage(atkPower);
                    yield return repeat;
                }
                else
                {
                    anim.SetTrigger(attack2Hash);
                    yield return wait2;
                    Player.Instance.TakeDamage(atkPower);
                    yield return repeat2;
                }
                CheckForTransition();
            }
        }
    }
}