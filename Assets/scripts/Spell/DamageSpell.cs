﻿using System;
using System.Collections.Generic;

namespace GameExtensions.Spells
{
    public class DamageSpell : Spell
    {
        public DamageSpell(string name, SpellType type, string description, byte lvl, int power, bool isUsedByPlayer,
            float targetAmount)
        {
            Name = name;
            Type = type;
            Description = description;
            Level = lvl;
            Power = power;
            Unlocked = isUsedByPlayer;
            TargetAmount = targetAmount;
        }

        protected override float TargetAmount { get; }
        public override string Name { get; }
        public override SpellType Type { get; }
        public override string Description { get; }
        public override byte Level { get; }
        public override int Power { get; }
        public override bool Unlocked { get; }

        public override void Use(IEnumerable<Entity> targets)
        {
            if (!Unlocked) throw new SpellNotUnlockedException();
            var realTargets = GetRealTargets(targets);
            foreach (var target in realTargets) target.TakeDamage(Power);
        }
    }

    internal class SpellNotUnlockedException : Exception
    {
    }
}