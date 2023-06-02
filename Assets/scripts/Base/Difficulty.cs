using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

namespace GameExtensions
{
    public static class Difficulty
    {
        public static byte CurrentDifficultyLevel { get; private set; }
        public static event UnityAction DifficultyLevelChanged;

        public static void ChangeDifficultyLevel(DifficultyLevel level)
        {
            CurrentDifficultyLevel = (byte)level;
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


