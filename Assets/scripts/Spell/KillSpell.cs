using System;
using System.Collections.Generic;
using System.Linq;
using GameExtensions;
using UnityEngine;

namespace GameExtensions.Spells
{
    public class KillSpell : Spell
    {
        protected override float TargetAmount { get; }
        public override string Name { get; }
        public override SpellType Type { get; }
        public override string Description { get; }
        public override byte Level { get; }
        public override int Power => int.MaxValue;
        public override bool Unlocked { get; }
        public KillSpell(string name,SpellType type,string description,byte lvl,bool isUsedByPlayer, float targetAmount)
        {
            Name = name;
            Type = type;
            Description = description;
            Level = lvl;
            Unlocked = isUsedByPlayer;
            TargetAmount = targetAmount;
        }
        public override void Use(IEnumerable<Entity> targets)
        {
            if (!Unlocked) throw new SpellNotUnlockedException();
            var realTargets = GetRealTargets(targets);
            foreach (var target in realTargets) target.Die();
        }
    }
}