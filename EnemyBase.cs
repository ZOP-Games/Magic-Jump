
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase : Entity
{
    //Base class for enemies, for things all enemies do
    protected Vector3 target;
    protected int atkRange = 50;

    protected override void Die()
    {
        Destroy(this);
    }

    protected float FindTargetAngle()
    {
        return Vector3.Angle(transform.position, target);
    }

    protected float FindTargetDistance()
    {
        return Vector3.Distance(transform.position, target);
    }

    protected virtual void Aim(float angle, float dist)
    {
        Transform.eulerAngles = PlayerTf.eulerAngles + 180;
        while (dist > atkRange && Mathf.Abs(rb.velocity.x) < 10 && Mathf.Abs(rb.velocity.z) < 10)
        {
            rb.AddRelativeForce(new Vector3(0, 0, 25));
        }

    }

    protected IEnumerator CheckPlayerPos(WaitForSeconds wfs)
    {
        Aim(FindTargetAngle(), FindTargetDistance());
        yield return wfs;
        CheckPlayerPos(wfs);
    }


    private void Start()
    {
        tag = "Enemy";
        WaitForSeconds wfs = new(0.25f);
        StartCoroutine(CheckPlayerPos(wfs));
    }
}
