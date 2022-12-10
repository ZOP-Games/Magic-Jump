using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameExtensions.Spells
{
    public class EnemyStunSpell : Spell
    {
        protected override float TargetAmount { get; }
        public override string Name { get; }
        public override SpellType Type {get; }
        public override string Description { get; }
        public override byte Level { get; }
        public override int Power => 0;
        public override bool Unlocked { get; }

        public EnemyStunSpell(string name, SpellType type, string description, byte level, bool isUsedByPlayer,float targetAmount)
        {
            Name = name;
            Type = type;
            Description = description;
            Level = level;
            Unlocked = isUsedByPlayer;
            TargetAmount = targetAmount;
        }
        public override void Use(IEnumerable<Entity> targets)
        {
            if (!Unlocked) throw new SpellNotUnlockedException();
            var realTargets = GetRealTargets(targets);
            foreach (var target in realTargets) target.Stun();
        }
    }
}