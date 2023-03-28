using System;
using System.Collections;
using System.Collections.Generic;
using GameExtensions.UI;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

namespace GameExtensions
{
    public class ScreenLayout : UIComponent
    {
        [SerializeField]private Scrollbar scroll;
        private float scrollDir;

        private void Start()
        {
            Debug.Assert(ES.currentInputModule is InputSystemUIInputModule,
                "Current Input Module is not of type InputSystemUIInputModule.");
            var input = ES.currentInputModule as InputSystemUIInputModule;
            input!.actionsAsset["Scroll"].Enable();
            input!.actionsAsset["Scroll"].performed += context =>
            {
                if(context.performed)scrollDir = context.ReadValue<Vector2>().y / 10;
                if (context.canceled) scrollDir = 0;
            };
        }

       private void FixedUpdate()
        {
            scroll.value += scrollDir;
        }
    }
}
