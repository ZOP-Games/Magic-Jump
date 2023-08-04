using GameExtensions.Debug;
using UnityEngine;

namespace GameExtensions.Story
{
    public abstract class StoryEvent : MonoBehaviour
    {
        public byte ProgressAmount => progressAmount;
        [SerializeField] protected byte progressAmount;
        protected byte id;
        [SerializeField] protected byte progressThreshold;

        protected abstract void DoEvent();

        protected virtual void Start()
        {
            Player.PlayerReady += () =>
            {
                DebugConsole.Log("Current progress: " + StoryProgression.Instance.Progress + "%");
                if (StoryProgression.Instance.Progress > progressThreshold + progressAmount) Destroy(gameObject);
            };
        }

        protected virtual void OnTriggerEnter(Collider other)
        {

            if (other is TerrainCollider || StoryProgression.Instance.Progress < progressThreshold) return;
            DoEvent();
        }
    }
}