using System.Collections.Generic;
using System.Collections;
using System.Linq;
using GameExtensions;

namespace GameExtensions.UI.Menus
{
    public class OptionsScreen : MenuScreen, IPassiveStart
    {
        protected override MenuScreen Parent => parent;
        private MenuScreen parent;

        public void SetParent(MenuScreen newParent)
        {
            parent = newParent;
        }

        public override void Open()
        {
            Controller.ActiveScreen = this;
            GObj.SetActive(true);
            if(Parent is null && PInput is not null) PInput.SwitchCurrentActionMap("UI");
        }
        
        public override void Close()
        {
            var menusToSave = GetComponentsInChildren<ISaveable>(true).Intersect(SaveManager.Savebles).ToList();
            foreach (var menu in menusToSave)
            {
                menu.Save();
            }
            base.Close();
        }

        public void PassiveStart()
        {
            DontDestroyOnLoad(transform.parent.gameObject);
        }
    }
}
