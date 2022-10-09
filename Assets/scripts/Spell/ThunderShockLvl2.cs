using UnityEngine;

namespace GameExtensions
{
    public class ThunderShockLvl2 : ThunderShock
    {
        public ThunderShockLvl2()
        {
            Name = "Thunder Shock - Level 2";
            Type = SpellType.Lightning;
            Description = "ph for thunder shock lvl 2";
            Level = 2;
        }


        public override void Use(Entity[] targets)
        {
            if (targets is not EnemyBase[] enemyTargets)
            {
                Debug.LogError("non-enemy entity in the target list!");
                return;
            }
            foreach (var target in enemyTargets)
            {
                target.Stun();
            }
        }
    }
}