using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Serialization;

// ReSharper disable UseNullPropagation

namespace GameExtensions.UI
{
    [RequireComponent(typeof(Image))]
    internal class InGameDialogBox : MenuScreen
    {
        [CanBeNull][SerializeField] private InGameDialogBox nextDialogBox;
        [SerializeField] private string title;
        [SerializeField] private string text;

        private TextMeshProUGUI TitleBox => GetComponentInChildren<TextMeshProUGUI>();
        private TextMeshProUGUI TextBox => GetComponentsInChildren<TextMeshProUGUI>()[1];
        public void Continue()
        {
            if (nextDialogBox is not null)
            {
                nextDialogBox.Open();
                GObj.SetActive(false);
            }
            else Close();

        }

        public override void Open()
        {
            Controller.ActiveScreen = this;
            GObj.SetActive(true);
            if(Parent is null && PInput.currentActionMap.name != "UI") PInput.SwitchCurrentActionMap("UI");
        }

        private void OnEnable()
        {
            TitleBox.SetText(title);
            TextBox.SetText(text);
        }
    }
}
