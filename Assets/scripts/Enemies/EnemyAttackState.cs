using System.Collections;
using System.Collections.Generic;
using GameExtensions.Debug;
using UnityEngine;

namespace GameExtensions.Enemies
{
    public class EnemyAttackState : EnemyState
    {
        protected readonly WaitForSeconds repeat;
        protected readonly WaitForSeconds wait;
        protected Animator anim;
        protected int atkPower;
        protected int attackHash;
        protected bool canAttack;
        private Transform playerTf;
        private Transform tf;

        public EnemyAttackState(EnemyStateManager enemy, float waitInterval, float repeatInterval) : base(enemy)
        {
            wait = new WaitForSeconds(waitInterval);
            repeat = new WaitForSeconds(repeatInterval);
        }

        protected override void CheckForTransition()
        {
            if (!canAttack) context.SetState(enemy.IdleState);
        }

        public override void Start()
        {
            base.Start();
            canAttack = true;
            anim = enemy.GetComponent<Animator>();
            atkPower = enemy.AtkPower;
            tf = context.transform;
            playerTf = Player.Instance.transform;
            attackHash = Animator.StringToHash("attack");
            context.StartCoroutine(nameof(Coroutine));
        }

        public override void ExitState()
        {
            context.StopCoroutine(nameof(Coroutine));
        }

        private IEnumerator Coroutine()
        {
            while (canAttack)
            {
                anim.SetTrigger(attackHash);
                yield return wait;
                Player.Instance.TakeDamage(atkPower);
                yield return repeat;
            }
        }

        public override void LateUpdate()
        {
            if (Vector3.Distance(tf.position, playerTf.position) < enemy.atkRange) return;
            canAttack = false;
            CheckForTransition();
        }

        public override void OnTriggerExit(Collider collider)
        {
            if (!collider.CompareTag("Player")) return;
            DontLookAtMe();
            canAttack = false;
            CheckForTransition();
        }
    }
}