using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using GameExtensions;
public class PauseScreen : MenuScreen
{
    private PlayerInput PInput
    {
        get;
        set;
    }
    protected override GameObject GObj => gameObject;
    public override bool IsActive { get; protected set; }

    public void Quit()
    {
        GameHelper.Quit();
    }

    public override void Close()
    {
        IsActive = false;
        GObj.SetActive(false);
        Time.timeScale = 1;
        InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInFixedUpdate;
        PInput.SwitchCurrentActionMap("Player");
    }

    public override void Open()
    {
        IsActive = true;
        InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInDynamicUpdate;
        PInput.SwitchCurrentActionMap("UI");
        Time.timeScale = 0;
        GObj.SetActive(true);
    }

    private void Start()
    {
        if (WarehouseFactory.Warehouse is null) return;
        wh = WarehouseFactory.Warehouse;
        wh.ActiveScreen = this;
        PInput = wh.PInput;
    }
}
