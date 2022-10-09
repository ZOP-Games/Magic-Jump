using UnityEngine;

namespace GameExtensions
{
    public class ThunderShockLvl3 : ThunderShock
    {
        public ThunderShockLvl3()
        {
            Name = "Thunder Shock - Level 3";
            Type = SpellType.Lightning;
            Description = "ph for thunder shock lvl 3";
            Level = 3;
        }

        public override void Use(Entity[] targets)
        {
            if (targets is not EnemyBase[] enemyTargets)
            {
                Debug.LogError("non-enemy entity in target list!");
                return;
            }

            foreach (var target in enemyTargets)
            {
                target.Die();
            }
        }
    }
}