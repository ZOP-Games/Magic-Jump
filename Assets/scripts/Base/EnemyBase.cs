using System;
using System.Collections.Generic;
using System.Collections;
using Cinemachine;
using Cinemachine.Utility;
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
        protected abstract Transform PlayerTransform { get; set; }
        protected const float TrackInterval = .02f;
        private bool isAttacking;
        private const float LookAtWeight = 0.1f;
        private const float LookAtRadius = 1;

        //death logic, just destroys itself
        public override void Die()
        {
            Player.Instance.AddXp(XpReward);
            DontLookAtMe(transform);
            Destroy(gameObject);
        }

        protected virtual void Aim()
        {
            var transform1 = transform;
            var pos = PlayerTransform.position;
            Debug.Log("Aiming towards " + pos);
            /*var distance = pos-transform1.position;
            var dot = Vector3.Dot(distance, transform1.up);
            var angle = Mathf.Acos(dot / (transform1.up.magnitude * distance.magnitude));
            transform1.Rotate(transform1.up,angle);
            Debug.Log("Turned " + angle);*/
            transform1.LookAt(new Vector3(pos.x,transform1.position.y,pos.z));
            if (Mathf.Abs(Vector3.Distance(transform1.position, pos)) > AtkRange)
            {
                CancelInvoke(nameof(Attack));
                isAttacking = false;
                Debug.Log("Moving forward");
                anim.SetBool(MovingPmHash,true);
                Move(transform1.InverseTransformDirection(transform1.forward));
            }
            else if(!isAttacking)
            {
                InvokeRepeating(nameof(Attack),0,2.5f);
                isAttacking = true;
            }
        }

        protected void StopAiming()
        {
            CancelInvoke(nameof(Attack));
            anim.SetBool(MovingPmHash,false);
            CancelInvoke(nameof(Aim));
            rb.Sleep();
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
            //CancelInvoke(nameof(Aim));
            base.Stun();
        }

        protected override void UnStun()
        {
            base.UnStun();
            //InvokeRepeating(nameof(TrackPlayer), 0, TrackInterval);
        }

        protected void Start()
        {
            //putting tag on enemy, helps w/ identification
            tag = "Enemy";
        }
    }
}