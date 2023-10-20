using System.Collections.Generic;
using GameExtensions.Debug;
using UnityEngine;

namespace GameExtensions.Enemies
{
    [RequireComponent(typeof(Collider))]
    public class EnemyLocation : MonoBehaviour
    {
        [SerializeField] private EnemyType type;
        private Color gizmoColor;
        private GameObject obj;
        private EnemyPools pools;
        private Vector3 position;
        private Transform tf;
        private int typeId;

        private void Start()
        {
            tf = transform;
            position = tf.position;
            pools = FindObjectOfType<EnemyPools>();
            typeId = (int) type;
        }

        private void OnDestroy()
        {
            if (obj is null) return;
            pools.ReturnInstance(obj, typeId);
        }

        private void OnDrawGizmosSelected()
        {
            if (gizmoColor.a is 0)
                gizmoColor = type switch
                {
                    EnemyType.Kwork => Color.blue,
                    EnemyType.Szellem => Color.cyan,
                    EnemyType.Sas => Color.magenta,
                    EnemyType.Lovag => Color.green,
                    _ => gizmoColor
                };
            else Gizmos.DrawIcon(transform.position, "gizmo.png", true, gizmoColor);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (obj is not null || !other.CompareTag("Player")) return;
            obj = pools.GetInstance(typeId, position);
            if (obj is null) return;
            tf.parent = obj.transform;
            obj.GetComponent<EnemyStateManager>().Reset();
            DebugConsole.Log("entered");
        }

        private void OnTriggerExit(Collider other)
        {
            if (obj is null || !other.CompareTag("Player")) return;
            pools.ReturnInstance(obj, typeId);
            obj = null;
            tf.parent = tf.root;
        }

        private enum EnemyType
        {
            Kwork,
            Szellem,
            Sas,
            Lovag
        }
    }
}