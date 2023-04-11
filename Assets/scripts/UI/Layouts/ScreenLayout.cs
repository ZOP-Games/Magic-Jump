using System;
using System.Collections;
using System.Collections.Generic;
using GameExtensions.UI;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;
using GameExtensions.Debug;

namespace GameExtensions.UI.Layouts
{
    public class ScreenLayout : UIComponent
    {
        [SerializeField]private Scrollbar scroll;
        [SerializeField]private GameObject firstObj;
        private float scrollDir;
        private void Start()
        {
            UnityEngine.Debug.Assert(ES.currentInputModule is InputSystemUIInputModule,
                "Current Input Module is not of type InputSystemUIInputModule.");
            var input = ES.currentInputModule as InputSystemUIInputModule;
            input!.actionsAsset["Scroll"].Enable();
            input!.actionsAsset["Scroll"].performed += context =>
            {
                if(context.performed)scrollDir = context.ReadValue<Vector2>().y / 10;
                if (context.canceled) scrollDir = 0;
            };
        }

        protected virtual void OnEnable()
        {
            DebugConsole.Log("setting selected obj", Color.blue);
            ES.SetSelectedGameObject(firstObj); //todo: fix object selection
        }

        private void FixedUpdate()
       {
           if (scroll.value is >= 1 or <= 0) return;
           scroll.value += scrollDir;
        }
    }
}
