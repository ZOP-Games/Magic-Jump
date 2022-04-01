using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : EnemyBase
{
    //we'll put navigation, animations + other things that are unique to this kind of enemy
    protected override int AttackStateHash => Animator.StringToHash("Attack");
    protected override int MoveStateHash => Animator.StringToHash("Move");

    protected override void Aim(float angle, float dist)
    {
        base.Aim(angle, dist);
    }

    protected override void Attack()
    {
        base.Attack();
    }
}
