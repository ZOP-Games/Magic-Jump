using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase : Entity
{
    //Base class for enemies, for things all enemies do
    protected Vector3 target;
    protected const int AtkRange = 0;
    [SerializeField] protected Transform playerTf;

    protected override void Die()
    {
        Destroy(this);
    }

    protected float FindTargetAngle()
    {
        //Debug.Log(transform.position + " " + target + " " + Mathf.Acos(Vector3.Dot(transform.position,target)));
        return Vector3.Angle(transform.position, target);
    }

    protected float FindTargetDistance()
    {
        return Vector3.Distance(transform.position, target);
    }

    protected virtual void Aim(float angle, float dist)
    {
        if (angle != 0)
        {
            transform.Rotate(0, angle, 0);

        }
        while (dist > AtkRange && Mathf.Abs(rb.velocity.x) < 10 && Mathf.Abs(rb.velocity.z) < 10)
        {
            rb.AddRelativeForce(new Vector3(0, 0, 25));
        }

        var transform1 = transform;
        Debug.Log("aiming, new rot: " + transform1.eulerAngles +", new pos: " + transform1.position);
    }
        
    protected void Check(){
    
        target = playerTf.position;
        Aim(FindTargetAngle(),FindTargetDistance());
    }
    protected virtual void Start()
    {
        tag = "Enemy";
    }


}
