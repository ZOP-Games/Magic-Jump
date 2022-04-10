using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase : Entity
{
    //Base class for enemies, for things all enemies do
    protected const int AtkRange = 0;
    [SerializeField] protected Transform playerTf;

    protected override void Die()
    {
        Destroy(this);
    }

    protected float FindTargetDistance()
    {
        return Vector3.Distance(transform.position, playerTf.position);
    }

    protected virtual void Aim(float dist)
    {
        transform.LookAt(playerTf);
        if (dist > AtkRange)
        {
            Move(Vector3.forward);
            
        }

        var transform1 = transform;
        Debug.Log("aiming, new rot: " + transform1.eulerAngles +", new pos: " + transform1.position);
    }
        
    protected void Check(){
        Aim(FindTargetDistance());
    }
    protected virtual void Start()
    {
        playerTf = GameObject.FindGameObjectWithTag("Player").transform;
        tag = "Enemy";
    }


}
