﻿using System.Collections.Generic;
using UnityEngine;

namespace GameExtensions.Story
{
    public sealed class StoryProgress
    {
        private StoryProgress() {}
        public static StoryProgress Instance => new();

        public byte Progress
        {
            get => progress;
            private set => progress = (byte) Mathf.Clamp(value, 0, 100);
        }

        private byte progress;

        private readonly Dictionary<int, byte> cutsceneProgressions = new()
        {
            {0, 1},
            {1, 2},
            {3, 50}
        };

        public void AddProgress(int lastCutscene)
        {
            Progress += cutsceneProgressions[lastCutscene];
        }

        
    }
}