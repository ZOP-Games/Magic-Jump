using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global



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

    //some components
    protected Rigidbody rb;
    protected Animator anim;

    protected TextMeshPro hpText;

    public void TakeDamage(int amount)
    {
        var ownName = name;
        Hp -= amount - Defense / 100;
        Debug.Log(ownName + " Took " + amount + " damage, current HP: " + Hp);
        hpText.SetText("HP: " + Hp);
        if (Hp > 0) return;
        Die();
        Debug.Log("Entity (" + ownName + ") died!");
    }

    // ReSharper disable once VirtualMemberNeverOverridden.Global
    protected virtual void Attack()
    {
        //this just starts the attack state, other attack logic is in OnCollisionEnter below
        if(!anim.GetCurrentAnimatorStateInfo(0).IsName("Attack")) anim.SetTrigger(AttackingPmHash);
        
    }

    protected void OnCollisionEnter(Collision collision)
    {
        //main attack logic, only runs if the attack state is active
        //if the attacked object is an Entity, it damages it
        var colliderHit = collision.collider;
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Attack") || !colliderHit.TryGetComponent(out Entity controller)) return;
        {
            controller.TakeDamage(AtkPower);
        }
    }

    protected abstract void Die();

    //common move method for all Entities
    protected void Move(Vector3 direction, int maxSpeed)
    {
        //Debug.Log(maxSpeed);
        //speed cap
        if (Mathf.Abs(rb.velocity.x) < maxSpeed && Mathf.Abs(rb.velocity.z) < maxSpeed)
        {
            rb.AddRelativeForce(direction.x * 25, 0, direction.z * 25);
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

}
