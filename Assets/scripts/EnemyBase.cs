using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase : Entity
{
    //Base class for enemies, for things all enemies do
    
    /*The enemy will only attack when the player is within this range (should be in meters,
    but because of scaling it's quite inconsistent, try to experiment with values)*/
    protected int AtkRange { get; set; }
    //WaitForSeconds object for enemies, this defines how often they Aim
    private const float LookAtHeight = 1.5f;
    private const float TrackInterval = .1f;
    //death logic, just destroys itself
    protected override void Die()
    {
        Destroy(gameObject);
    }

    
    //aiming logic: finding the player, moving towards it and start attacking
    // ReSharper disable once VirtualMemberNeverOverridden.Global
    protected virtual void Aim(Transform playerTf, int offset)
    {
        var transform1 = transform;
        var position = playerTf.position;
        //setting angle, looking at the player's transform
        var fixedPos = new Vector3(position.x, LookAtHeight, position.z);
        transform1.LookAt(fixedPos);
        transform1.Rotate(0,offset,0);  //rotation offset because zoli
        //setting position, moving until the player is within range
        if (Mathf.Abs(Vector3.Distance(transform1.position, playerTf.position)) > AtkRange)
        {
            InvokeRepeating(nameof(TrackPlayer),0,TrackInterval);
        }
        else
        {
            //if it's in range, it attacks
            CancelInvoke(nameof(TrackPlayer));
            anim.SetBool(MovingPmHash, false);
            //Debug.Log("enemy is attacking");
            Attack(AtkSpherePos,AtkSphereRadius);
            
        }
    }
    //stop aiming fix, sets the rigidbody to kinematic so it will stop moving towards the player
    protected void StopAiming()
    {
        CancelInvoke(nameof(TrackPlayer));
        anim.SetBool(MovingPmHash,false);
        rb.isKinematic = true;
    }
    //checking coroutine, wrapper for Aim()    
    private void TrackPlayer()
    {
        anim.SetBool(MovingPmHash,true);
        Move(Vector3.right); //I'm not dumb anymore yay! (but zoli is)
        //Debug.Log("aiming, new rot: " + transform1.eulerAngles +", new pos: " + transform1.position);
    }



    protected void Start()
    {
        //putting tag on enemy, helps w/ identification
        tag = "Enemy";
    }


}
