using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using GameExtensions.Debug;
using TMPro;

namespace GameExtensions.UI.Menus
{
    public class AlertScreen : MenuScreen
    {

        public string HeaderText => Header.text;
        public string BodyText => Body.text;
        public string ButtonText => Button.text;

        public TextMeshProUGUI Header { get; private set;}
        public TextMeshProUGUI Body { get; private set;}
        public TextMeshProUGUI Button { get; private set;}

        public static void CreateAlert(string headerText = "header text", string bodyText = "body text",
            string buttonText = "OK")
        {
            var alert = FindObjectOfType<AlertScreen>(true);
            var txts = alert.GetComponentsInChildren<TextMeshProUGUI>();
            alert.Header = txts[0];
            alert.Body = txts[1];
            alert.Button = txts[2];
            alert.Header.SetText(headerText);
            alert.Body.SetText(bodyText);
            alert.Button.SetText(buttonText);
            alert.Open();
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
            if (PInput is null) DebugConsole.LogError("There is no PlayerInput provided to AlertScreen.");
            InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInDynamicUpdate;
            PInput!.SwitchCurrentActionMap("UI");
            if(Time.timeScale > 0) Time.timeScale = 0;
            base.Open();
        }

        public void Start()
        {
            Player.PlayerReady += () => Player.Instance.AddInputAction("Show Objective", () =>
            {
                if(Player.Instance.PInput.currentControlScheme != "Controller") return;
                var saveLoadBindings = Player.Instance.PInput.actions.Where(a => a.name is "Save" or "Load")
                    .Select(a => a.GetBindingDisplayString()).ToArray();
                CreateAlert("You pressed the save/load button!",
                    "Press " + saveLoadBindings[0] + " to save and " + saveLoadBindings[1] + " to load the game!");
            },IInputHandler.ActionType.Canceled);
            gameObject.SetActive(false);
        }
    }
}
