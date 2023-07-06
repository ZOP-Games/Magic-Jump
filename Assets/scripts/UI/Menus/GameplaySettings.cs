using GameExtensions.UI.HUD;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameExtensions.UI.Menus
{
    public class GameplaySettings : ScreenLayout, ISaveable, IPassiveStart
    {
        [field: SerializeField] public Difficulty.DifficultyLevel CurrentDifficultyLevel { get; private set; }
        [field: SerializeField] public bool IsHUDEnabled { get; private set; }

        [SerializeField] private TMP_Dropdown difficultyDropdown;
        [SerializeField] private Toggle hudToggle;

        private new void Start()
        {
            difficultyDropdown.value = (int)Difficulty.CurrentDifficultyLevel;
            hudToggle.SetIsOnWithoutNotify(IsHUDEnabled);
            firstObj = difficultyDropdown.gameObject;
            base.Start();
            OnEnable();
        }


        public void PassiveStart()
        {
            Difficulty.ChangeDifficultyLevel(CurrentDifficultyLevel);
            HUDToggler.AskSetHUD(IsHUDEnabled);
        }

        byte ISaveable.Id { get; set; }

        public void ChangeDifficultyLevel(int levelNumber)
        {
            levelNumber = Mathf.Clamp(levelNumber, 0, 3);
            CurrentDifficultyLevel = (Difficulty.DifficultyLevel)levelNumber;
            Difficulty.ChangeDifficultyLevel(CurrentDifficultyLevel);
        }

        public void ChangeHUDVisibility(bool newValue)
        {
            IsHUDEnabled = newValue;
            HUDToggler.AskSetHUD(IsHUDEnabled);
        }
    }
}