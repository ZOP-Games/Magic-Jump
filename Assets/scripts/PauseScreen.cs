using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using GameExtensions;
using UnityEditor.SceneManagement;
using UnityEngine.UI;

public class PauseScreen : MenuScreen
{
    private static PlayerInput PInput => PlayerInput.GetPlayerByIndex(0);
    private EventSystem ES => EventSystem.current;
    protected override GameObject GObj => gameObject;
    public override bool IsActive { get; protected set; }
    public override event EventHandler Opened;
    public override event EventHandler Closed;

    public void Quit()
    {
        GameHelper.Quit();
    }

    public override void Close()
    {
        Closed?.Invoke(this,EventArgs.Empty);
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
        ES.SetSelectedGameObject(GetComponentInChildren<Button>().gameObject);
        Debug.Log(ES.currentSelectedGameObject.name);
        Opened?.Invoke(this,EventArgs.Empty);
    }
}
