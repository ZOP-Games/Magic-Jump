using GameExtensions.Debug;
using UnityEngine;
using UnityEngine.Events;

namespace GameExtensions
{
    public class DamageState : State
    {
        private readonly int damagePmHash = Animator.StringToHash("damage");

        private Animator anim;
        private readonly EntityStateManager entity;
        
        public event UnityAction HealthChanged;

        public DamageState(EntityStateManager context) : base(context)
        {
            entity = context;
        }

        protected override void CheckForTransition()
        {
        }

        public override void Start()
        {
            anim = entity.GetComponent<Animator>();
        }

        //damage logic, the dealt damage is substracted from Enitity's HP
        public void TakeDamage(int amount)
        {
            anim.SetTrigger(damagePmHash);
            entity.Hp -= Mathf.Clamp(amount - entity.Defense / 100, 0, amount);
            HealthChanged?.Invoke();
            DebugConsole.Log("That was " + amount + " damage!");
            if (entity.Hp > 0) return; //if the Entity has 0 HP, it dies
            context.SetState(EntityStateManager.DeathState);
        }
    }
}