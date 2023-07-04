using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GameExtensions.UI.Menus
{
    public class ControlRemappingScreen : ColumnContainer
    {
        public static List<InputAction> actions;
        [SerializeField] private InputActionAsset inputActionAsset;
        [SerializeField] private GameObject gOjb;

        private void Start()
        {
            actions = inputActionAsset.ToList();
            
        }
    }
}