using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    protected abstract Vector3 AtkSpherePos { get;}
    protected abstract int AtkSphereRadius { get; }

    //some components
    protected Rigidbody rb;
    protected Animator anim;

    protected TextMeshPro hpText;

    protected const int WalkSpeed = 5;
    protected const int RunSpeed = 15;
    protected const int MoveForceMultiplier = 25;
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
    protected virtual void Attack(Vector3 spherePosOffset,int radius)
    {
        //docs here
        anim.SetTrigger(AttackingPmHash);
        var colliders = new Collider[16];
        var atkPos = transform.localPosition + spherePosOffset;
        Physics.OverlapSphereNonAlloc(atkPos, radius, colliders);
        var collidersList = colliders.ToList();
        collidersList.RemoveAll(c => c is null || !c.TryGetComponent(out Entity controller) || controller == this);
        collidersList.ForEach(c =>
        {
            //Debug.Log(name + " Hit collider! name: " + c.name + ", index: " + collidersList.IndexOf(c));
            c.GetComponent<Entity>().TakeDamage(AtkPower);
        });
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.localPosition + AtkSpherePos,AtkSphereRadius);
    }

    protected abstract void Die();

    //common move method for all Entities
    protected void Move(Vector3 direction, int maxSpeed)
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

}
