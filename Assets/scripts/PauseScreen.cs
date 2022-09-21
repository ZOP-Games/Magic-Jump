using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using GameExtensions;
public class PauseScreen : MenuScreen
{
    [SerializeField]private PlayerInput pInput;
    protected override PlayerInput PInput => pInput;
    protected override GameObject GObj => gameObject;

    public void Quit()
    {
        GameHelper.Quit();
    }

    public override void Close()
    {
        GObj.SetActive(false);
        Time.timeScale = 1;
        InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInFixedUpdate;
        PInput.SwitchCurrentActionMap("Player");
    }

    public override void Open()
    {
        InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInDynamicUpdate;
        PInput.SwitchCurrentActionMap("UI");
        Time.timeScale = 0;
        GObj.SetActive(true);
    }
}
