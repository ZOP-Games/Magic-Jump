using System.Collections.Generic;
using System.Collections;
using GameExtensions;
using UnityEngine.UI;

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
    }
}
