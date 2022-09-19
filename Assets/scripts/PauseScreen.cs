using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseScreen : MenuScreen
{
    [SerializeField]private PlayerInput pInput;
    protected override PlayerInput PInput => pInput;
}
