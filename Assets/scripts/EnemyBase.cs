using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase : Entity
{
    //Base class for enemies, for things all enemies do
    protected const int AtkRange = 10;
    protected readonly WaitForSeconds wfs = new (0.2f);
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
        if (Mathf.Abs(dist) > AtkRange)
        {
            Move(Vector3.up,5); //up because me dumb
            
        }
        else
        {
            Attack();
        }

        var transform1 = transform;
        Debug.Log("aiming, new rot: " + transform1.eulerAngles +", new pos: " + transform1.position);
    }
        
    protected IEnumerator Check(){
        Aim(FindTargetDistance());
        yield return wfs;
    }
    protected virtual void Start()
    {
        playerTf = GameObject.FindGameObjectWithTag("Player").transform;
        tag = "Enemy";
    }


}
