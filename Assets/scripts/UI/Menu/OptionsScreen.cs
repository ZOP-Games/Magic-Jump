using System.Collections.Generic;
using System.Collections;
using System.Linq;
using GameExtensions;

namespace GameExtensions.UI.Menus
{
    public class OptionsScreen : MenuScreen
    {
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
    }
}
