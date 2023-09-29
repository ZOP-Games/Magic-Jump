using GameExtensions.Debug;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace GameExtensions.Enemies
{
    public class EnemyPools : MonoBehaviour
    {
        [SerializeField] private List<EnemyPoolPreset> presets;
        private List<Queue<GameObject>> pools;

        [CanBeNull]
        public GameObject GetInstance(int typeId, Vector3 position)
        {
            if (pools.Count - 1 < typeId)
            {
                DebugConsole.Log("Spawning cancelled: There is no EnemyPool with the given id.",
                 DebugConsole.WarningColor);
                return null;
            }
            var queue = pools[typeId];
            if (queue.Count == 0)
            {
                DebugConsole.Log("over enemy limit");
                return null;
            }
            var obj = queue.Dequeue();
            obj.transform.position = position;
            obj.SetActive(true);
            DebugConsole.Log("placed object (" + obj.name + ") from pool, we have " + queue.Count + " left");
            return obj;
        }

        public void ReturnInstance(GameObject instance, int typeId)
        {
            instance.SetActive(false);
            pools[typeId].Enqueue(instance);
            DebugConsole.Log("Object returned to pool, we have " + pools[typeId].Count + " left");
        }

        private void Start()
        {
            pools = new List<Queue<GameObject>>();
            presets.ForEach(p =>
            {
                var pool = new Queue<GameObject>(p.Size);
                for (var i = 0; i < p.Size; i++)
                {
                    var obj = Instantiate(p.Prefab, Vector3.zero, Quaternion.identity, transform);
                    obj.SetActive(false);
                    pool.Enqueue(obj);
                }
                pools.Add(pool);
            });
            DebugConsole.Log("filled up " + pools.Count + " pools");
        }

    }
}


