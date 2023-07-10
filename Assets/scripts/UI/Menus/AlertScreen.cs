using System.Collections;
using System.Collections.Generic;
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

        public static AlertScreenBuilder CreateAlert(){
            var obj = new GameObject("Alert Screen");
            var alert = obj.AddComponent<AlertScreen>();
            alert.Header = obj.AddComponent<TextMeshProUGUI>();
            alert.Body = obj.AddComponent<TextMeshProUGUI>();
            var bObj = new GameObject();
            var btn = bObj.AddComponent<UnityEngine.UI.Button>();
            alert.Button = bObj.AddComponent<TextMeshProUGUI>();
            bObj.transform.parent = obj.transform;
            _ = obj.AddComponent<ColumnContainer>();
            btn.onClick.AddListener(() => {
                alert.Close();
                Destroy(obj);
                });
            return new AlertScreenBuilder(alert);
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
            base.Open();
        }
    }
}
