using GameExtensions.Debug;
using UnityEngine;

namespace GameExtensions.Story
{
    [System.Serializable]
    public sealed class StoryProgression : ISaveable
    {
        [SerializeField] private byte progress;

        private StoryProgression()
        {
            (this as ISaveable).AddToList();
        }

        public static StoryProgression Instance { get; } = new();

        public byte Progress => progress;
        byte ISaveable.Id { get; set; }

        public void AddProgress(byte progressPercent)
        {
            if (progress + progressPercent > 100)
            {
                DebugConsole.Log("Added progress makes progression more than 100%. Addition will be ignored", DebugConsole.WarningColor);
                return;
            }
            if (progressPercent > 10) DebugConsole.Log("Major progress detected (" + progressPercent
                + "%). Are you sure about this?", DebugConsole.WarningColor);
            progress += progressPercent;
            DebugConsole.Log("Added " + progressPercent + "% progress, overall progress: " + Progress + "%");
        }
    }
}