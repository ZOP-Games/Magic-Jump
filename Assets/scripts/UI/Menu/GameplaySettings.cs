using System.Collections;
using System.Collections.Generic;
using GameExtensions.Debug;
using TMPro;
using UnityEngine;


namespace GameExtensions.UI.Menus
{
    public class GameplaySettings : ScreenLayout, ISaveable, IPassiveStart
    {
        byte ISaveable.Id { get; set; }

        [field: SerializeField] public Difficulty.DifficultyLevel CurrentDifficultyLevel { get; private set; }
        [field: SerializeField] public bool IsHUDEnabled { get; private set; }

        [SerializeField] private TMP_Dropdown difficultyDropdown;


        public void ChangeDifficultyLevel(int levelNumber)
        {
            levelNumber = Mathf.Clamp(levelNumber, 0, 3);
            CurrentDifficultyLevel = (Difficulty.DifficultyLevel)levelNumber;
            Difficulty.ChangeDifficultyLevel(CurrentDifficultyLevel);
        }

        private new void Start()
        {
            difficultyDropdown.value = (int)Difficulty.CurrentDifficultyLevel;
            base.Start();
        }


        public void PassiveStart()
        {
            Difficulty.ChangeDifficultyLevel(CurrentDifficultyLevel);
        }
    }

}