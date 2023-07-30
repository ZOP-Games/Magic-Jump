using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GameExtensions.OpenWorld
{
    public class ChunkBorder : MonoBehaviour
    {

        [SerializeField] private AssetReferenceGameObject otherChunk;
        private Transform tf;
        private bool isNewChunkLoaded;

        private void OnTriggerExit(Collider other)
        {
            if (!isNewChunkLoaded)
            {
                otherChunk.InstantiateAsync(tf.root).Completed += _ =>
                {
                    Debug.Log("made chunk");
                    isNewChunkLoaded = true;
                };
            }
            else
            {
                Destroy(gameObject);
                Addressables.ReleaseInstance(gameObject);
                Debug.Log("destroyed chunk");
                isNewChunkLoaded = false;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(new Vector3(50,10,90),new Vector3(100,20,0));
        }

        private void Start()
        {
            tf = transform;
        }
    }
}
