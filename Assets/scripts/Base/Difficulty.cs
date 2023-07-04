using System.Collections;
using System.Collections.Generic;
using GameExtensions.Debug;
using UnityEngine;
using UnityEngine.Events;

namespace GameExtensions
{
    public static class Difficulty
    {
        
        public static event UnityAction DifficultyLevelChanged;

        public static DifficultyLevel CurrentDifficultyLevel { get; private set; }
        public static float DifficultyMultiplier => DifficultyMultipliers[(int)CurrentDifficultyLevel];

        private static readonly float[] DifficultyMultipliers =
        {
            0.5f,
            1,
            1.5f,
            2f
        };

        public static void ChangeDifficultyLevel(DifficultyLevel level)
        {
            CurrentDifficultyLevel = level;
            DifficultyLevelChanged?.Invoke();
        }

        public enum DifficultyLevel
        {
            Easy,
            Medium,
            Hard,
            Intense
        }
    }
}


