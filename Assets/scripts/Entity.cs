using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    // this tell any entity we might have (the player, enemies, etc.) what they all can do
    public int Hp { get; set; } = 100;
    public int AtkPower { get; set; } = 10;
    public int Defense { get; set; } = 10;

    protected abstract int AttackStateHash { get; }
    protected abstract int MoveStateHash { get; }
    protected Rigidbody rb;
    protected Animator anim;

    public void TakeDamage(int amount)
    {
        Hp -= (amount - Defense / 100);
        Debug.Log("Took " + amount + " damage, current HP: " + Hp);
        if (Hp <= 0)
        {
            Die();
            Debug.Log("Entity (" + name + ") died!");
        }
    }

    protected virtual void Attack()
    {
        anim.SetBool(AttackStateHash, true);
    }

    public void OnCollisionEnter(Collision collision)
    {
        //replace new Animator w/ character's animator -> GetComponent<Animator>()
        if (anim.GetBool(AttackStateHash) && TryGetComponent(out Entity controller))
        {
            controller.TakeDamage(AtkPower);

        }

    }

    protected abstract void Die();

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

    }

}
