using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : EnemyBase
{
    //we'll put navigation, animations + other things that are unique to this kind of enemy (the bird)

    //setting the attack and moving state hash and attack range
    protected override int AttackingPmHash => Animator.StringToHash("attacking");
    protected override int MovingPmHash => Animator.StringToHash("moving");
    protected override int AtkRange { get; } = 10;

    
    //if the player enters the aim trigger, it starts the Check coroutine
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(Check(other.transform,-90));
        }
    }
    //if the player leaves the aim trigger, it stops the Check coroutine and applies the stop aiming fix
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        StopCoroutine(Check(other.transform,-90));
        StopAiming();
    }


    protected new void Start()
    {
        //setting the attack stat for the enemy and getting some components from the gameobject
        AtkPower = 1;
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

    }
}
