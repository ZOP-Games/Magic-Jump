using UnityEngine;

namespace GameExtensions.Story
{
    public class StoryProgression
    {
        public byte Progress
        {
            get => progress;
            set => progress = (byte) Mathf.Clamp(value, 0, 100);
        }
        

        private byte progress;
    }
}