using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace GameExtensions
{
    public abstract class Spell
    {
        public abstract string Name { get; }
        public abstract SpellType Type { get; }
        public abstract string Description { get; }
        public abstract byte Level { get; }
        public abstract int Power { get; }
        public abstract bool Unlocked { get; }
        protected abstract float TargetAmount { get; }
        public abstract void Use(IEnumerable<Entity> targets);

        protected IEnumerable<Entity> GetRealTargets(IEnumerable<Entity> targets)
        {
            var targetList = targets.ToList();
            targetList = targetList.Where(e => e is EnemyBase).ToList();
            Debug.Log("I have " + targetList.Count + " targets");
            if (TargetAmount < 1)
            {
                var realAmount = (int)(targetList.Count * TargetAmount);
                return targetList.Skip(Random.Range(0, (targetList.Count + 1) - realAmount)).Take(realAmount);
            }
            else
            {
                var realAmount = (int)TargetAmount;
                return targetList.Skip(Random.Range(0, (targetList.Count + 1) - realAmount)).Take(realAmount);
            }
        }
    }

    public enum SpellType
    {
        Ice = 0,
        Lightning = 1,
        Fire = 2,
        Wind = 3,
        Grass = 4,
        Earth = 5,
        Dark = 6,
        Other = 7
    }
}
