using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase : Entity
{
    //Base class for enemies, for things all enemies do
    protected abstract int AtkRange { get; }
    protected readonly WaitForSeconds wfs = new (0.5f);

    //death logic, just destroys itself
    protected override void Die()
    {
        Destroy(gameObject);
    }

    
    //aiming logic: finding the player, moving towards it and start attacking
    protected virtual void Aim(Transform playerTf)
    {
        //aim stop fix
        if (rb.isKinematic)
        {
            rb.isKinematic = false;
        }
        anim.SetBool(MovingPmHash,true);
        var transform1 = transform;
        //setting angle, looking at the player's transform
        transform1.LookAt(playerTf);
        transform1.Rotate(0,-90,0);
        //setting position, moving until the player is within range
        if (Mathf.Abs(Vector3.Distance(transform1.position, playerTf.position)) > AtkRange)
        {
            Move(Vector3.up,5); //up because me dumb
            
        }
        else
        {
            //if it's in range, it attacks
            Attack();
        }
        Debug.Log("aiming, new rot: " + transform1.eulerAngles +", new pos: " + transform1.position);
    }
    //stop aiming fix
    protected void StopAiming()
    {
        rb.isKinematic = true;
    }
    //checking coroutine, wrapper for Aim()    
    protected IEnumerator Check(Transform playerTf){
        Aim(playerTf);
        yield return wfs;
    }
    protected void Start()
    {
        //putting tag on enemy, helps w/ identification
        tag = "Enemy";
    }


}
