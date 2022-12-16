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
    /// <summary>
    /// Represents a dialog box of a <see cref="NonPlayer"/> which contains the text it's saying. 
    /// </summary>
    [RequireComponent(typeof(Image))]
    internal class InGameDialogBox : MenuScreen
    {
        /// <summary>
        /// The next <see cref="InGameDialogBox"/> the <see cref="NonPlayer"/> will say after continuing.
        /// </summary>
        /// <remarks>This can be null.</remarks>
        [CanBeNull][SerializeField] private InGameDialogBox nextDialogBox;
        /// <summary>
        /// The title of the box.
        /// </summary>
        private string title;
        /// <summary>
        /// The text of the box.
        /// </summary>
        [SerializeField] private string text;

        /// <summary>
        /// The <see cref="TextMeshProUGUI"/> box containing the title.
        /// </summary>
        private TextMeshProUGUI TitleBox => GetComponentInChildren<TextMeshProUGUI>();
        /// <summary>
        /// The <see cref="TextMeshProUGUI"/> box containing the text.
        /// </summary>
        private TextMeshProUGUI TextBox => GetComponentsInChildren<TextMeshProUGUI>()[1];

        private NonPlayer Owner => GetComponentInParent<NonPlayer>();
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
            title = Owner.characterName;
            TitleBox.SetText(title);
            TextBox.SetText(text);
        }
    }
}
