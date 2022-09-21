using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class OptionsScreen : MenuScreen
{
    [SerializeField] private PlayerInput pInput;
    protected override PlayerInput PInput => pInput;
    protected override GameObject GObj => gameObject;

}
