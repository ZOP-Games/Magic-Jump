using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameExtensions.Nonplayer
{
    public class InGameListBox : MenuScreen, IContinuable
    {
        [SerializeField]private string[] options;
        private InGameDialogBox DialogBox => GetComponentInParent<InGameDialogBox>();
        protected override MenuScreen Parent => DialogBox;
        /// <summary>
        /// The next <see cref="IContinuable"/> the <see cref="NonPlayer"/> will show after continuing.
        /// </summary>
        /// <remarks>This can be null.</remarks>
        [CanBeNull] private MenuScreen NextBox => transform.GetNextSibling() is not null ? transform.GetNextSibling().GetComponent<IContinuable>() as MenuScreen: null;

        public override void Open()
        {
            Controller.ActiveScreen = this;
            GObj.SetActive(true);
            var texts = GetComponentsInChildren<Button>().Select(b => b.GetComponentInChildren<TextMeshProUGUI>()).ToList();
            for(var i = 0;i < texts.Count;i++)
            {
                try
                {
                    texts.ElementAt(i).SetText(options[i]);
                }
                catch
                {
                    texts.ElementAt(i).SetText("Box " + i);
                }

                
            }
            var firstButton = GetComponentInChildren<Button>();
            if(firstButton is not null) ES.SetSelectedGameObject(firstButton.gameObject);
        }

        public void Continue()
        {
            Debug.Log("option selected: " + ES.currentSelectedGameObject.name);

            if (NextBox is not null)
            {
               NextBox.Open();
                GObj.SetActive(false);
            }
            else Close();
        }
    }
}