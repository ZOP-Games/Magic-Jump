using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : EnemyBase
{
    //we'll put navigation, animations + other things that are unique to this kind of enemy
    protected override int AttackingPmHash => Animator.StringToHash("attacking");
    protected override int MovingPmHash => Animator.StringToHash("moving");

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(Check());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StopCoroutine(Check());
        }
    }

    protected new void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        
            
    }
}
