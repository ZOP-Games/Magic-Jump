using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GameExtensions.OpenWorld
{
    public class ChunkBorder : MonoBehaviour
    {

        [SerializeField] private AssetReferenceGameObject otherChunk;
        [SerializeField] private Vector3Int id;
        private Transform tf;
        private bool isNewChunkLoaded;

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            var goingIn = Vector3.Dot(other.attachedRigidbody.velocity.normalized, tf.localPosition.normalized) < 0;
            if (goingIn)
            {
                Debug.Log("going in");
                Destroy(tf.root.gameObject);
                Addressables.Release(tf.root.gameObject);
                Debug.Log("destroying " + id);
            }
            else
            {
                otherChunk.InstantiateAsync(tf.root, true).Completed += op =>
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
