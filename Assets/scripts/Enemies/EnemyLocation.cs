using GameExtensions.Debug;
using UnityEngine;

namespace GameExtensions.Enemies
{
    [RequireComponent(typeof(Collider))]
    public class EnemyLocation : MonoBehaviour
    {
        [SerializeField] private EnemyType type;
        private int typeId;
        private Vector3 position;
        private EnemyPools pools;
        private GameObject obj;
        private Color gizmoColor;
        private Transform tf;

        private void OnTriggerEnter(Collider other)
        {
            if (obj is not null || !other.CompareTag("Player")) return;
            obj = pools.GetInstance(typeId, position);
            if(obj is null) return;
            tf.parent = obj.transform;
            obj.GetComponent<EnemyBase>().Reset();
            DebugConsole.Log("entered");
        }

        private void OnTriggerExit(Collider other)
        {
            if(obj is null || !other.CompareTag("Player")) return;
            pools.ReturnInstance(obj, typeId);
            obj = null;
            tf.parent = tf.root;
        }

        private void Start()
        {
            tf = transform;
            position = tf.position;
            pools = FindObjectOfType<EnemyPools>();
            typeId = (int)type;
        }

        private void OnDestroy()
        {
            if (obj is null) return;
            pools.ReturnInstance(obj, typeId);
        }

        private void OnDrawGizmos()
        {
            if (gizmoColor.a is 0)
            {
                switch (type)
                {
                    case EnemyType.Kwork:
                        gizmoColor = Color.blue;
                        break;
                    case EnemyType.Szellem:
                        gizmoColor = Color.cyan;
                        break;
                    case EnemyType.Sas:
                        gizmoColor = Color.magenta;
                        break;
                    case EnemyType.Lovag:
                        gizmoColor = Color.green;
                        break;
                }
            }
            else Gizmos.DrawIcon(tf.position,"gizmo.png", true, gizmoColor);
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
