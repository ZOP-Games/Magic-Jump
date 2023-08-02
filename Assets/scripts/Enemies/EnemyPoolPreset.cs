using UnityEngine;

namespace GameExtensions.Enemies
{
    [System.Serializable]
    public class EnemyPoolPreset
    {
        public GameObject Prefab => prefab;
        public byte Size => size;

        [SerializeField] private GameObject prefab;
        [SerializeField] private byte size;
    }
}