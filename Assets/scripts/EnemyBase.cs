using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public abstract class EnemyBase : Entity
{
    //Base class for enemies, for things all enemies do
    protected abstract int AtkRange { get; }
    protected readonly WaitForSeconds wfs = new (0.2f);

    protected override void Die()
    {
        Destroy(this);
    }

    

    protected virtual void Aim(Transform playerTf)
    {
        if (rb.isKinematic)
        {
            rb.isKinematic = false;
        }

        var transform1 = transform;
        transform1.LookAt(playerTf);
        //transform1.eulerAngles += new Vector3(0, offset, 0);
        if (Mathf.Abs(Vector3.Distance(transform.position, playerTf.position)) > AtkRange)
        {
            Move(Vector3.up,5); //up because me dumb
            
        }
        else
        {
            Attack();
        }

        
        Debug.Log("aiming, new rot: " + transform1.eulerAngles +", new pos: " + transform1.position);
    }

    protected void StopAiming()
    {
        rb.isKinematic = true;
    }
        
    protected IEnumerator Check(Transform playerTf){
        Aim(playerTf);
        yield return wfs;
    }
    protected void Start()
    {
        tag = "Enemy";
    }


}
