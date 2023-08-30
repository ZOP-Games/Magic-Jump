using System.Collections.Generic;
using System.Linq;
using GameExtensions.Debug;
using UnityEngine;
using UnityEngine.Events;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace GameExtensions
{
    [RequireComponent(typeof(CharacterController), typeof(Animator))]
    public abstract class Entity : MonoBehaviour
    {
        //some constants
        protected const int WalkSpeed = 5;
        protected const int RunSpeed = 15;
        protected const int MoveForceMultiplier = 25;
        protected const int StunTime = 3;

        //public stats, so anyone can read them and set them
        [field: SerializeField] public int Hp { get; protected set; } = 100;

        // ReSharper disable once MemberCanBeProtected.Global
        [field: SerializeField] public int AtkPower { get; set; } = 10;

        [field: SerializeField] public int Defense { get; set; } = 10;
        protected Animator anim;

        private Vector3 atkPos; //position of the attack sphere

        //some components
        protected Rigidbody rb;
        protected CharacterController cc;

        //hashes of animator state names, use these for state checks
        protected abstract int AttackingPmHash { get; }
        protected abstract int MovingPmHash { get; }
        protected abstract int RunningPmHash { get; }
        protected abstract int DamagePmHash { get; }
        protected abstract Vector3 AtkSpherePos { get; } //position of the Enitity's hitbox
        protected abstract int AtkRange { get; } //the radius of the hitbox sphere
        protected float DifficultyMultiplier { get; set; }

        private string OwnName => name; //name of the Entity

        protected void Start()
        {
            DifficultyMultiplier = Difficulty.DifficultyMultiplier;
            Difficulty.DifficultyLevelChanged += () => DifficultyMultiplier = Difficulty.DifficultyMultiplier;
        }

        //draws the hitbox sphere in scene view
        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position + AtkSpherePos, AtkRange);
        }

        // this tells any entity we might have (the player, enemies, etc.) what they all can do
        public event UnityAction HealthChanged;

        //damage logic, the dealt damage is substracted from Enitity's HP
        public virtual void TakeDamage(int amount)
        {
            anim.SetTrigger(DamagePmHash);
            Hp -= Mathf.Clamp(amount - Defense / 100, 0, amount);
            HealthChanged?.Invoke();
            DebugConsole.Log("That was " + amount + " damage!");
            if (Hp > 0) return; //if the Entity has 0 HP, it dies
            Die();
        }

        // ReSharper disable once VirtualMemberNeverOverridden.Global
        protected virtual void Attack()
        {
            //docs here
            anim.SetTrigger(AttackingPmHash);
            var entities = FindObjectsByType<Entity>(FindObjectsSortMode.None).Where(e => e.isActiveAndEnabled
             && Mathf.Abs(Vector3.Distance(e.transform.position, transform.position)) < AtkRange
             && Vector3.Dot(e.transform.forward,transform.forward) < 0
             && e != this
            )
            .ToList();
            foreach (var entity in entities)
            {
                entity.TakeDamage(AtkPower);
            }
            DebugConsole.Log("attacked " + entities.Count() + " entities");
        }

        public IEnumerable<T> GetNearby<T>() where T : Component
        {
            anim.SetTrigger(AttackingPmHash);
            var hits = FindObjectsByType<T>(FindObjectsSortMode.None)
            .Where(e =>
                Mathf.Abs(Vector3.Distance(e.transform.position, transform.position)) < AtkRange && e != this)
            .ToList();
            return hits;
        }

        public abstract void Die();

        //common move method for all Entities
        protected void Move(Vector3 direction, int maxSpeed = 5)
        {
            anim.SetBool(MovingPmHash, true);
            if (maxSpeed > 5) anim.SetBool(RunningPmHash, true);
            cc.Move(new Vector3(direction.x * MoveForceMultiplier, direction.y, direction.z * MoveForceMultiplier)
            );
        }

        protected void Move(Vector3 direction, bool isRunning)
        {
            anim.SetBool(MovingPmHash, true);
            anim.SetBool(RunningPmHash, isRunning);
            cc.Move(new Vector3(direction.x * MoveForceMultiplier, direction.y, direction.z * MoveForceMultiplier));
        }

        public virtual void Stun()
        {
            cc.enabled = false;
            Invoke(nameof(UnStun), StunTime);
        }

        protected virtual void UnStun()
        {
            cc.enabled = true;
        }
    }
}