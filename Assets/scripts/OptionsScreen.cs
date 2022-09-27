using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameExtensions;
using UnityEngine;
using UnityEngine.InputSystem;

public class OptionsScreen : MenuScreen
{
    protected override GameObject GObj => gameObject;
    public override bool IsActive { get; protected set; }
    public override event EventHandler Opened;
    public override event EventHandler Closed;

    public override void Close()
    {
        Closed?.Invoke(this,EventArgs.Empty);
        base.Close();
    }

    private void OnEnable()
    {
        Opened?.Invoke(this,EventArgs.Empty);
    }
}
