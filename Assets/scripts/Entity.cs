using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global


[RequireComponent(typeof(Rigidbody),typeof(Animator),typeof(Collider))]
public abstract class Entity : MonoBehaviour
{
    // this tells any entity we might have (the player, enemies, etc.) what they all can do

    //public stats, so anyone can read them and set them
    public int Hp { get; set; } = 100;

    // ReSharper disable once MemberCanBeProtected.Global
    public int AtkPower { get; set; } = 10;

    public int Defense { get; set; } = 10;

    //hashes of animator state names, use these for state checks
    protected abstract int AttackingPmHash { get; }

    protected abstract int MovingPmHash { get; }
    protected abstract Vector3 AtkSpherePos { get;} //position of the Enitity's hitbox
    protected abstract int AtkSphereRadius { get; } //the radius of the hitbox sphere
    private string OwnName => name; //name of the Entity
    //some components
    protected Rigidbody rb;
    protected Animator anim;
    protected TextMeshPro hpText;

    private Vector3 atkPos; //position of the attack sphere
    //some constants
    protected const int WalkSpeed = 5;
    protected const int RunSpeed = 15;
    protected const int MoveForceMultiplier = 25;
    protected const int StunTime = 3;
    //damage logic, the dealt damage is substracted from Enitity's HP
    public void TakeDamage(int amount)
    {
        Hp -= amount - Defense / 100;
        Debug.Log(OwnName + " Took " + amount + " damage, current HP: " + Hp);
        hpText.SetText("HP: " + Hp);
        if (Hp > 0) return; //if the Entity has 0 HP, it dies
        Die();
        Debug.Log("Entity (" + OwnName + ") died!");
    }

    // ReSharper disable once VirtualMemberNeverOverridden.Global
    protected virtual void Attack()
    {
        //docs here
        anim.SetTrigger(AttackingPmHash);
        var collidersList = GetEntities(AtkSpherePos,AtkSphereRadius);
        collidersList.ForEach(c =>
        {
            //Debug.Log(name + " Hit collider! name: " + c.name + ", index: " + collidersList.IndexOf(c));
            c.GetComponent<Entity>().TakeDamage(AtkPower); //take damage
        });
    }

    protected List<Entity> GetEntities(Vector3 spherePosOffset,int radius)
    {
        atkPos = transform.localPosition + spherePosOffset; //position of the hitbox
        var colliders = new Collider[16];   //an array of colliders we store hit objects in
        Physics.OverlapSphereNonAlloc(atkPos, radius, colliders);   //creating the hitbox sphere and colllecting colliders inside
        var entities = colliders.Where(c => c is not null).Select(c => c.GetComponent<Entity>()).ToList();
        entities.RemoveAll(c => c is null || c == this);  //removing nulls and the attacking Entity itself
        return entities;
    }

    //draws the hitbox sphere in scene view
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.localPosition + AtkSpherePos,AtkSphereRadius);
    }

    public abstract void Die();

    //common move method for all Entities
    protected void Move(Vector3 direction, int maxSpeed = 5)
    {
        //Debug.Log(maxSpeed);
        //speed cap
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
        Invoke(nameof(UnStun),StunTime);
    }

    protected virtual void UnStun()
    {
        rb.isKinematic = false;
    }
}
