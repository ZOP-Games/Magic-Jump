using GameExtensions.UI;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// ReSharper disable UseNullPropagation

namespace GameExtensions.Nonplayer
{
    /// <summary>
    ///     Represents a dialog box of a <see cref="NonPlayer" /> which contains the text it's saying.
    /// </summary>
    [RequireComponent(typeof(Image))]
    public class InGameDialogBox : MenuScreen, IContinuable
    {
        [SerializeField] private string title;

        /// <summary>
        ///     The text of the box.
        /// </summary>
        [SerializeField] private string text;

        private IInteractable owner;

        /// <summary>
        ///     The next <see cref="IContinuable" /> the <see cref="NonPlayer" /> will show after continuing.
        /// </summary>
        /// <remarks>This can be null.</remarks>
        [CanBeNull]
        private MenuScreen NextBox => transform.GetNextSibling() is not null
            ? transform.GetNextSibling().GetComponent<IContinuable>() as MenuScreen
            : null;

        /// <summary>
        ///     The title of the box.
        /// </summary>
        public string Title
        {
            get => title;
            set => title = value;
        }

        public string Text
        {
            get => text;
            set => text = value;
        }

        /// <summary>
        ///     The <see cref="TextMeshProUGUI" /> box containing the title.
        /// </summary>
        private TextMeshProUGUI TitleBox => GetComponentInChildren<TextMeshProUGUI>();

        /// <summary>
        ///     The <see cref="TextMeshProUGUI" /> box containing the text.
        /// </summary>
        private TextMeshProUGUI TextBox => GetComponentsInChildren<TextMeshProUGUI>()[1];

        public void Continue()
        {
            if (NextBox is not null)
            {
                NextBox.Open();
                GObj.SetActive(false);
            }
            else
            {
                Close();
            }
        }

        public override void Open()
        {
            if (PInput is null) Debug.LogError("There is no PlayerInput provided to PauseScreen.");
            Controller.ActiveScreen = this;
            GObj.SetActive(true);
            owner = GetComponentInParent<IInteractable>();
            title = owner.OwnName;
            if (Parent is null && PInput!.currentActionMap.name != "UI") PInput.SwitchCurrentActionMap("UI");
            TitleBox.SetText(title);
            TextBox.SetText(text);
        }
    }
}