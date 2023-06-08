using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using GameExtensions.Debug;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace GameExtensions 
{
    [RequireComponent(typeof(Rigidbody), typeof(Animator), typeof(Collider))]
    public abstract class Entity : MonoBehaviour
    {
        // this tells any entity we might have (the player, enemies, etc.) what they all can do
        public event UnityAction HealthChanged;

        //public stats, so anyone can read them and set them
        [field:SerializeField]public int Hp { get; protected set; } = 100;

        // ReSharper disable once MemberCanBeProtected.Global
        [field:SerializeField]public int AtkPower { get; set; } = 10;

        [field:SerializeField]public int Defense { get; set; } = 10;

        //hashes of animator state names, use these for state checks
        protected abstract int AttackingPmHash { get; }
        protected abstract int MovingPmHash { get; }
        protected abstract int RunningPmHash { get; }
        protected abstract int DamagePmHash { get; }
        protected abstract Vector3 AtkSpherePos { get; } //position of the Enitity's hitbox
        protected abstract int AtkSphereRadius { get; } //the radius of the hitbox sphere
        protected float DifficultyMultiplier { get; set; }

        //some components
        protected Rigidbody rb;
        protected Animator anim;

        //some constants
        protected const int WalkSpeed = 5;
        protected const int RunSpeed = 15;
        protected const int MoveForceMultiplier = 25;
        protected const int StunTime = 3;

        private string OwnName => name; //name of the Entity

        private Vector3 atkPos; //position of the attack sphere

        //damage logic, the dealt damage is substracted from Enitity's HP
        public void TakeDamage(int amount)
        {
            anim.SetTrigger(DamagePmHash);
            Hp -= Mathf.Clamp(amount - Defense / 100,0,amount);
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
                .Where(c => c is not null && c != this && !c.CompareTag(tag)).ToList(); //removing nulls and the attacking Entity itself and Entities of the same type
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

        //draws the hitbox sphere in scene view
        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.localPosition + AtkSpherePos, AtkSphereRadius);
        }

        public abstract void Die();

        //common move method for all Entities
        protected void Move(Vector3 direction, int maxSpeed = 5)
        {
            //Debug.Log(maxSpeed);
            //speed cap
            anim.SetBool(MovingPmHash,true);
            if(maxSpeed > 5) anim.SetBool(RunningPmHash,true);
            if (Mathf.Abs(rb.velocity.x) < maxSpeed && Mathf.Abs(rb.velocity.z) < maxSpeed)
            {
                rb.AddRelativeForce(direction.x * MoveForceMultiplier, 0, direction.z * MoveForceMultiplier);
            }
            else
            {
                //if the Entity is faster than it's moving, it slows it down
                var velocity = rb.velocity;
                if (Mathf.Abs(velocity.x) > maxSpeed)
                {
                    velocity.x = maxSpeed * Mathf.Sign(velocity.x);
                }

                if (Mathf.Abs(velocity.z) > maxSpeed)
                {
                    velocity.z = maxSpeed * Mathf.Sign(velocity.z);
                }

                rb.velocity = velocity;
            }
        }

        public virtual void Stun()
        {
            rb.isKinematic = true;
            Invoke(nameof(UnStun), StunTime);
        }

        protected virtual void UnStun()
        {
            rb.isKinematic = false;
        }

        protected void Start()
        {
            DifficultyMultiplier = Difficulty.DifficultyMultiplier;
            Difficulty.DifficultyLevelChanged += () => DifficultyMultiplier = Difficulty.DifficultyMultiplier;
        }
    }
}
