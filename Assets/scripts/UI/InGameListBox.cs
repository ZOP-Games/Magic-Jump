using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace GameExtensions.UI
{
    [RequireComponent(typeof(InGameDialogBox))]
    public class InGameListBox : MenuScreen
    {
        [SerializeField] private List<Button> options;
        private InGameDialogBox DialogBox => GetComponentInParent<InGameDialogBox>();
        protected override MenuScreen Parent => DialogBox;

        public override void Open()
        {
            Controller.ActiveScreen = this;
            GObj.SetActive(true);
            //options.ForEach(button => button.onClick.AddListener(SelectOption(options.IndexOf(button))));
            var firstButton = options.First();
            if(firstButton is not null) ES.SetSelectedGameObject(firstButton.gameObject);
        }

        private void SelectOption(int selNumber)
        {
            //todo:figure out how we handle selections
        }
    }
}