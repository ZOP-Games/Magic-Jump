using UnityEngine;

namespace GameExtensions
{
    public class ThunderShock : Spell
    {
        public ThunderShock()
        {
            Name = "Thunder Shock";
            Type = SpellType.Lightning;
            Description = "A lightning strike from the magic wand stuns enemies.";
            Level = 1;
        }

        public override void Use(Entity[] targets)
        {
            var index = Random.Range(0, targets.Length);
            if (targets is not EnemyBase[] enemyTargets)
            {
                Debug.LogError("non-enemy entity in the target list!");
                return;
            }

            enemyTargets[index].Stun();

        }
    }
}