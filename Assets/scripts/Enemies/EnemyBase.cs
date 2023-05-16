using System;
using System.Collections.Generic;
using System.Collections;
using Cinemachine;
using TMPro;
using UnityEngine;

namespace GameExtensions.Enemies
{
    /// <summary>
    /// Base class for all enemies.
    /// </summary>
    [RequireComponent(typeof(LODGroup))]
    public abstract class EnemyBase : Entity
    {
        //Base class for enemies, for things all enemies do

        /*The enemy will only attack when the player is within this range (should be in meters,
        but because of scaling it's quite inconsistent, try to experiment with values)*/
        protected int AtkRange { get; set; }
        protected TextMeshPro hpText;
        protected abstract Vector3 ForwardDirection { get; }
        protected abstract float Height { get; }
        protected abstract byte XpReward { get; }
        protected abstract Transform PlayerTransform { get; set; }
        protected abstract float AtkRepeatRate { get; }
        protected abstract CinemachineTargetGroup Ctg { get; set; }
        protected const float TrackInterval = .02f;
        protected bool isAttacking;
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
            transform1.LookAt(new Vector3(pos.x,transform1.position.y,pos.z));
            if (Mathf.Abs(Vector3.Distance(transform1.position, pos)) > AtkRange)
            {
                CancelInvoke(nameof(Attack));
                isAttacking = false;
                Move(transform1.InverseTransformDirection(transform1.forward));
            }
            else if(!isAttacking)
            {
                InvokeRepeating(nameof(Attack),0,AtkRepeatRate);
                isAttacking = true;
            }
        }

        protected void StopAiming()
        {
            CancelInvoke(nameof(Attack));
            anim.SetBool(MovingPmHash,false);
            anim.SetBool(RunningPmHash,false);
            CancelInvoke(nameof(Aim));
            rb.Sleep();
        }

        protected void LookAtMe(Transform target)
        {
            Ctg.AddMember(target, LookAtWeight, LookAtRadius);
        }

        protected void DontLookAtMe(Transform target)
        {
            Ctg.RemoveMember(target);
        }

        protected void Start()
        {
            //putting tag on enemy, helps w/ identification
            tag = "Enemy";
        }
    }
}