using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using GameExtensions;
using TMPro;

public class PauseScreen : MenuScreen
{
    private TextMeshProUGUI XpText => GetComponentsInChildren<TextMeshProUGUI>().Last();
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
        var xpInfo = MenuController.Controller.XpInfo;
        XpText.SetText("Level: {0}\nXP: {1}\nXP needed to level up: {2}",xpInfo.Item3,xpInfo.Item1,xpInfo.Item2-xpInfo.Item1);
        base.Open();
    }

    private void OnEnable()
    {
        Debug.Log("Xp info: " + MenuController.Controller.XpInfo);
    }
}
