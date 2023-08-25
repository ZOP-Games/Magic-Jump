using System.Collections.Generic;
using System.Linq;
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
        protected abstract int AtkSphereRadius { get; } //the radius of the hitbox sphere
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
            Gizmos.DrawWireSphere(transform.position + AtkSpherePos, AtkSphereRadius);
        }

        // this tells any entity we might have (the player, enemies, etc.) what they all can do
        public event UnityAction HealthChanged;

        //damage logic, the dealt damage is substracted from Enitity's HP
        public virtual void TakeDamage(int amount)
        {
            anim.SetTrigger(DamagePmHash);
            Hp -= Mathf.Clamp(amount - Defense / 100, 0, amount);
            HealthChanged?.Invoke();
            if (Hp > 0) return; //if the Entity has 0 HP, it dies
            Die();
        }

        // ReSharper disable once VirtualMemberNeverOverridden.Global
        protected virtual void Attack()
        {
            //docs here
            anim.SetTrigger(AttackingPmHash);
            var collidersList = GetNearbyEntities();
            collidersList.ForEach(c =>
            {
                c.GetComponent<Entity>().TakeDamage(AtkPower); //take damage
            });
        }

        protected List<Entity> GetNearbyEntities()
        {
            atkPos = transform.localPosition + AtkSpherePos; //position of the hitbox
            var colliders = new Collider[16]; //an array of colliders we store hit objects in
            Physics.OverlapSphereNonAlloc(atkPos, AtkSphereRadius,
                colliders); //creating the hitbox sphere and colllecting colliders inside
            var entities = colliders.Where(c => c is not null).Select(c => c.GetComponent<Entity>())
                .Where(c => c is not null && c != this && !c.CompareTag(tag))
                .ToList(); //removing nulls and the attacking Entity itself and Entities of the same type
            return entities;
        }

        public IEnumerable<T> GetNearby<T>() where T : Component
        {
            atkPos = transform.localPosition + AtkSpherePos; //position of the hitbox
            var colliders = new Collider[16]; //an array of colliders we store hit objects in
            Physics.OverlapSphereNonAlloc(atkPos, AtkSphereRadius,
                colliders); //creating the hitbox sphere and colllecting colliders inside
            var hits = colliders.Where(c => c is not null).Select(c => c.GetComponent<T>()).ToList();
            hits.RemoveAll(c => c is null); //removing nulls
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

        protected void Move(Vector3 direction,bool isRunning){
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