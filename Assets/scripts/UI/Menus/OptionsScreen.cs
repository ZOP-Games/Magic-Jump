using System.Linq;

namespace GameExtensions.UI.Menus
{
    public class OptionsScreen : MenuScreen, IPassiveStart
    {
        private MenuScreen parent;
        protected override MenuScreen Parent => parent;

        public void PassiveStart()
        {
            DontDestroyOnLoad(transform.parent.gameObject);
        }

        public void SetParent(MenuScreen newParent)
        {
            parent = newParent;
        }

        public override void Open()
        {
            Controller.ActiveScreen = this;
            GObj.SetActive(true);
            if (Parent is null && PInput is not null) PInput.SwitchCurrentActionMap("UI");
        }

        public override void Close()
        {
            var menusToSave = GetComponentsInChildren<ISaveable>(true).Intersect(SaveManager.Savebles).ToList();
            foreach (var menu in menusToSave) menu.Save();
            base.Close();
        }
    }
}