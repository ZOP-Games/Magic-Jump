using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameExtensions.Enemies
{
    public class EnemyGroup
    {
        public EnemyGroup(IEnumerable<EnemyBase> enemies, IEnumerable<Vector3> locations)
        {
            var posList = locations.ToList();
            EnemyLocations = new Dictionary<EnemyBase, Vector3>();
            foreach (var enemy in enemies)
            foreach (var pos in posList)
                EnemyLocations.Add(enemy, pos);
        }

        public Dictionary<EnemyBase, Vector3> EnemyLocations { get; }

        public IEnumerable<EnemyBase> Enemies => EnemyLocations.Select(e => e.Key);
        //todo: do the rest of the enemy spawning setup (see spell impl. for details)
    }
}