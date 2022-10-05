using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using GameExtensions;

public class PauseScreen : MenuScreen
{
    private static PlayerInput PInput => PlayerInput.GetPlayerByIndex(0);

    public void Quit()
    {
        GameHelper.Quit();
    }

    public override void Close()
    {
        base.Close();
        Time.timeScale = 1;
        InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInFixedUpdate;
        PInput.SwitchCurrentActionMap("Player");
        
    }

    public override void Open()
    {
        InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInDynamicUpdate;
        PInput.SwitchCurrentActionMap("UI");
        Time.timeScale = 0;
        base.Open();
    }
}
