using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    // this tells any entity we might have (the player, enemies, etc.) what they all can do
    public int Hp { get; set; } = 100;
    public int AtkPower { get; set; } = 10;
    public int Defense { get; set; } = 10;

    protected abstract int AttackingPmHash { get; }
    protected abstract int MovingPmHash { get; }
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
        anim.SetBool(AttackingPmHash, true);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!anim.GetBool(AttackingPmHash) || !collision.gameObject.TryGetComponent(out Entity controller)) return;
        controller.TakeDamage(AtkPower);

    }

    protected abstract void Die();

    protected void Move(Vector3 direction, int maxSpeed)
    {
        //Debug.Log(maxSpeed);
        if (Mathf.Abs(rb.velocity.x) < maxSpeed && Mathf.Abs(rb.velocity.z) < maxSpeed)
        {
            rb.AddRelativeForce(direction.x * 25, 0, direction.y * 25);
        }
        else 
        {
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
