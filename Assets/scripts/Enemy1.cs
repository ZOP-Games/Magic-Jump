using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : EnemyBase
{
    //we'll put navigation, animations + other things that are unique to this kind of enemy
    protected override int AttackStateHash => Animator.StringToHash("Attack");
}
