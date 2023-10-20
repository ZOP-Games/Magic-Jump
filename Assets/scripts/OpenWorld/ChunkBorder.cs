using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GameExtensions.OpenWorld
{
    public class ChunkBorder : MonoBehaviour
    {

        [SerializeField] private AssetReferenceGameObject otherChunk;
        [SerializeField] private Vector3Int id;
        private Transform tf;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            var dot = Vector3.Dot(other.transform.position.normalized, tf.position.normalized);
            Debug.Log("Player is " + dot + " from me, that's " + Mathf.Acos(dot) + "°");
            var goingIn = dot < 0;
            if (goingIn)
            {
                Debug.Log("going in");
                var root = tf.root;
                Destroy(root.gameObject);
                Addressables.Release(root.gameObject);
                Debug.Log("destroying " + id);
            }
            else
            {
                otherChunk.InstantiateAsync(tf.root, true).Completed += _ =>
                {
                    Debug.Log("made chunk by: " + id);
                };
            }

        }

        private void Start()
        {
            tf = transform;
        }
    }
}
