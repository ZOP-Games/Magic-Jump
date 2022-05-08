using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase : Entity
{
    //Base class for enemies, for things all enemies do
    
    /*The enemy will only attack when the player is within this range (should be in meters,
    but because of scaling it's quite inconsistent, try to experiment with values)*/
    protected abstract int AtkRange { get; }
    //WaitForSeceons object for enemies, this defines how often they Aim
    protected readonly WaitForSeconds wfs = new (0.5f);

    //death logic, just destroys itself
    protected override void Die()
    {
        Destroy(gameObject);
    }

    
    //aiming logic: finding the player, moving towards it and start attacking
    // ReSharper disable once VirtualMemberNeverOverridden.Global
    protected virtual void Aim(Transform playerTf, int offset)
    {
        //aim stop fix
        if (rb.isKinematic)
        {
            rb.isKinematic = false;
        }
        var transform1 = transform; 
        //setting angle, looking at the player's transform
        transform1.LookAt(playerTf);
        transform1.Rotate(0,offset,0);  //rotation offset because zoli
        //setting position, moving until the player is within range
        if (Mathf.Abs(Vector3.Distance(transform1.position, playerTf.position)) > AtkRange)
        {
            anim.SetBool(MovingPmHash,true);
            Move(Vector3.right,5); //I'm not dumb anymore yay! (but zoli is)
        }
        else if(!anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            //if it's in range, it attacks
            anim.SetBool(MovingPmHash, false);
            Attack();
            
        }
        Debug.Log("aiming, new rot: " + transform1.eulerAngles +", new pos: " + transform1.position);
    }
    //stop aiming fix, sets the rigidbody to kinematic so it will stop moving towards the player
    protected void StopAiming()
    {
        rb.isKinematic = true;
    }
    //checking coroutine, wrapper for Aim()    
    protected IEnumerator Check(Transform playerTf, int offset){
        Aim(playerTf,offset);
        yield return wfs;
    }



    protected void Start()
    {
        //putting tag on enemy, helps w/ identification
        tag = "Enemy";
    }


}
