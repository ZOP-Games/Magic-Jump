using System.Collections.Generic;
using System.Collections;
using Cinemachine;
using UnityEngine;

namespace GameExtensions 
{
    [RequireComponent(typeof(LODGroup))]
    public abstract class EnemyBase : Entity
    {
        //Base class for enemies, for things all enemies do
        [SerializeField] private CinemachineTargetGroup ctg;

        /*The enemy will only attack when the player is within this range (should be in meters,
        but because of scaling it's quite inconsistent, try to experiment with values)*/
        protected int AtkRange { get; set; }
        protected abstract Vector3 ForwardDirection { get; }
        protected abstract float Height { get; }
        protected abstract byte XpReward { get; }
        private const float TrackInterval = .5f;
        private const float LookAtWeight = 0.1f;
        private const float LookAtRadius = 1;

        //death logic, just destroys itself
        public override void Die()
        {
            Player.Instance.AddXp(XpReward);
            DontLookAtMe(transform);
            Destroy(gameObject);
        }


        //aiming logic: finding the player, moving towards it and start attacking
        // ReSharper disable once VirtualMemberNeverOverridden.Global
        protected virtual void Aim(Transform playerTf, int offset) //todo: try setting up AI navigation instead
        {
            var transform1 = transform;
            var position = playerTf.position;
            //setting angle, looking at the player's transform
            var fixedPos = new Vector3(position.x, Height, position.z);
            transform1.LookAt(fixedPos);
            transform1.Rotate(0, offset, 0); //rotation offset because zoli
            //setting position, moving until the player is within range
            if (Mathf.Abs(Vector3.Distance(transform1.position, playerTf.position)) > AtkRange)
            {
                InvokeRepeating(nameof(TrackPlayer), 0, TrackInterval);
            }
            else
            {
                //if it's in range, it attacks
                CancelInvoke(nameof(TrackPlayer));
                anim.SetBool(MovingPmHash, false);
                //Debug.Log("enemy is attacking");
                Attack();
            }
        }

        //stop aiming fix, sets the rigidbody to kinematic so it will stop moving towards the player
        protected void StopAiming()
        {
            CancelInvoke(nameof(TrackPlayer));
            anim.SetBool(MovingPmHash, false);
            rb.isKinematic = true;
        }

        //checking coroutine, wrapper for Aim()    
        private void TrackPlayer()
        {
            anim.SetBool(MovingPmHash, true);
            Move(ForwardDirection); //I'm not dumb anymore yay! (but zoli is)
            //Debug.Log("aiming, new rot: " + transform1.eulerAngles +", new pos: " + transform1.position);
        }


        protected void LookAtMe(Transform target)
        {
            ctg.AddMember(target, LookAtWeight, LookAtRadius);
        }

        protected void DontLookAtMe(Transform target)
        {
            ctg.RemoveMember(target);
        }

        public override void Stun()
        {
            CancelInvoke(nameof(TrackPlayer));
            base.Stun();
        }

        protected override void UnStun()
        {
            base.UnStun();
            InvokeRepeating(nameof(TrackPlayer), 0, TrackInterval);
        }

        protected void Start()
        {
            //putting tag on enemy, helps w/ identification
            tag = "Enemy";
        }
    }
}