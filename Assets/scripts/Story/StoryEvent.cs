using UnityEngine;
using GameExtensions;

namespace GameExtensions.Story
{
    public abstract class StoryEvent : MonoBehaviour, ITriggerable
    {
        public byte ProgressAmount => progressAmount;
        [SerializeField] private byte progressAmount;
        protected byte id;
        [SerializeField] protected byte progressThreshold;

        public void Trigger()
        {
            gameObject.SetActive(true);
        }

        protected abstract void DoEvent();

        protected virtual void OnTriggerEnter(Collider other)
        {
            if(other is TerrainCollider) return;
            DoEvent();
            //Destroy(gameObject);
        }
    }
}