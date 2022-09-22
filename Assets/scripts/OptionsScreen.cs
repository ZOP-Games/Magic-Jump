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

    private void Start()
    {
        if (WarehouseFactory.Warehouse is null) return;
        wh = WarehouseFactory.Warehouse;
    }
}
