using GameExtensions.Debug;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GameExtensions.UI.Menus
{
    public class PauseScreen : OptionsParentScreen
    {
        [SerializeField] private TextMeshProUGUI xpText;

        public void Quit()
        {
            GameHelper.Quit();
        }

        public override void Close()
        {
            base.Close();
            Time.timeScale = 1;
            InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInFixedUpdate;
            PInput!.SwitchCurrentActionMap("Player");
        }

        public override void Open()
        {
            if (PInput is null) DebugConsole.LogError("There is no PlayerInput provided to PauseScreen.");
            InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInDynamicUpdate;
            PInput?.SwitchCurrentActionMap("UI");
            if(Time.timeScale > 0) Time.timeScale = 0;
            var xpInfo = MenuController.Controller.XpInfo;
            xpText.SetText("Level: {0}\nXP: {1}\nXP needed to level up: {2}", xpInfo.Item3, xpInfo.Item1,
                xpInfo.Item2 - xpInfo.Item1);
            DebugConsole.Log("Hey all, stuck here,");
            base.Open();
        }


        public void Save()
        {
            SaveManager.SaveAll();
        }

        public void Load()
        {
            SaveManager.LoadAll();
        }
    }
}