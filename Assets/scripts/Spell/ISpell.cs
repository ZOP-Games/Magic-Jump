using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace GameExtensions
{
    public interface ISpell
    {
        public string Name { get; }
        public SpellType Type { get;}
        public string Description { get; }
        public byte Level { get; }
        public int Power { get; }
        public bool Unlocked { get; }

        public void Use(IEnumerable<Entity> targets);

        public IEnumerable<Entity> GetRealTargets(IEnumerable<Entity> targets)
        {
            var targetList = targets.ToList();
            targetList = targetList.Where(e => e is EnemyBase).ToList();
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
        public float TargetAmount { get; }
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
