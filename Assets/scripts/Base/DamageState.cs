using System.Collections.Generic;
using GameExtensions.Debug;
using UnityEngine;

namespace GameExtensions
{
    public class DamageState : NotifyableState<int>
    {
        private readonly int damagePmHash = Animator.StringToHash("damage");
        private readonly EntityStateManager entity;
        private Animator anim;

        private int damageAmount;

        public DamageState(EntityStateManager context) : base(context)
        {
            entity = context;
        }

        public override void Notify(int data)
        {
            damageAmount = data;
        }

        protected override void CheckForTransition()
        {
        }

        public override void Start()
        {
            anim = entity.GetComponent<Animator>();
            TakeDamage(damageAmount);
        }

        //damage logic, the dealt damage is substracted from Enitity's HP
        private void TakeDamage(int amount)
        {
            if (damageAmount == 0)
                DebugConsole.Log(
                    "TakeDamage has been called without notifying DamageState first." +
                    " Please use EntityStateManager.Mediate to take damage correctly.", DebugConsole.WarningColor);
            anim.SetTrigger(damagePmHash);
            entity.Hp -= Mathf.Clamp(amount - entity.Defense / 100, 0, amount);
            DebugConsole.Log(context.name + " took " + amount + " damage!");
            context.SetState(entity.Hp <= 0 ? entity.DeathState : entity.IdleState);    //if the Entity has 0 HP, it dies
        }
    }
}