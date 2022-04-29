using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : EnemyBase
{
    //we'll put navigation, animations + other things that are unique to this kind of enemy
    protected override int AttackingPmHash => Animator.StringToHash("attacking");
    protected override int AtkRange { get; } = 10;

    protected override int MovingPmHash => Animator.StringToHash("moving");
    
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(Check(other.transform));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        StopCoroutine(Check(other.transform));
        StopAiming();
    }


    protected new void Start()
    {
        AtkPower = 1;
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

    }
}
