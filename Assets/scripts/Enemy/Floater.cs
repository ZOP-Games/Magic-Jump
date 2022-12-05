using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Floater : EnemyBase
{
    //class for the Floater enemy, it's the same as enemy1 for now
    protected override int AttackingPmHash => Animator.StringToHash("attacking");
    protected override int MovingPmHash => Animator.StringToHash("moving");
    protected override Vector3 AtkSpherePos => Vector3.zero;
    protected override int AtkSphereRadius => 3;
    protected override Vector3 ForwardDirection => Vector3.forward;

    protected override float Height => 3;
    protected override byte XpReward => 12;
    private const int TurnOffset = 0;

    private readonly WaitForSeconds wfs = new (0.5f);

    private bool canCheck;
    //if the player enters the aim trigger, it starts the Check coroutine
    private IEnumerator Check(Collider other)
    {
        Aim(other.transform,TurnOffset);
        yield return wfs;
        if(canCheck) StartCoroutine(Check(other));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (rb.isKinematic)
        {
            rb.isKinematic = false;
        }
        if (!other.CompareTag("Player")) return;
        LookAtMe(transform);
        StartCoroutine(Check(other));
        canCheck = true;
    }

    //if the player leaves the aim trigger, it stops the Check coroutine and applies the stop aiming fix
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        DontLookAtMe(transform);
        StopCoroutine(Check(other));
        canCheck = false;
        StopAiming();
    }


    protected new void Start()
    {
        //setting the attack stat for the enemy and getting some components from the gameobject
        AtkRange = 10;
        AtkPower = 1;
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        hpText = GetComponentInChildren<TextMeshPro>();
        hpText.SetText("HP: 100");
    }
}
